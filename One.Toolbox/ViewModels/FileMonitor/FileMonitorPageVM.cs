using Microsoft.Extensions.DependencyInjection;

using One.Core.Helpers;
using One.Toolbox.ExtensionMethods;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;

using Vanara.Extensions;

using static Vanara.PInvoke.RstrtMgr;

using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace One.Toolbox.ViewModels.FileMonitor;

public partial class FileMonitorPageVM : BaseVM
{
    public FileMonitorPageVM()
    {
    }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData()
    {
    }

    [RelayCommand]
    private void Drop(object obj)
    {
        //List<Uri>? Last = obj as List<Uri>;
        //Uri? tem = Last?.Last();

        //FilePath = tem.LocalPath;

        TestFile(FilePath);
    }

    [RelayCommand]
    private void DropTest(DragEventArgs drgevent)
    {
        drgevent.Handled = true;

        // Check that the data being dragged is a file
        if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Get an array with the filenames of the files being dragged
            string[] files = (string[])drgevent.Data.GetData(DataFormats.FileDrop);

            FilePath = files.Last();
        }
        else
            drgevent.Effects = DragDropEffects.None;

        Debug.WriteLine(DateTime.Now);
        Task.Run(() =>
        {
           App.Current.Dispatcher.Invoke(() =>
           {
               TestFile(FilePath);
           });
        });
        Debug.WriteLine(DateTime.Now);
    }

    [ObservableProperty]
    private string filePath;

    public ObservableCollection<FileInUseVM> ProcessList { get; set; } = new ObservableCollection<FileInUseVM>();

    private void TestFile(string filePath)
    {
        ProcessList.Clear();

        StringBuilder stringBuilder = new StringBuilder();
        RmStartSession(out uint pSessionHandel, 0, stringBuilder).Judge();

        RmRegisterResources(pSessionHandel, 1, [filePath], 0, null, 0, null).Judge();

        uint nProcInfo = 10;
        RM_PROCESS_INFO[] rM_PROCESS_INFOs = new RM_PROCESS_INFO[nProcInfo];
        RmGetList(
                pSessionHandel,
                out uint nProcInfoNeeded,
                ref nProcInfo,
                rM_PROCESS_INFOs,
                out RM_REBOOT_REASON lpdwRebootReasons
            )
            .Judge();

        if (nProcInfoNeeded == 0)
        {
            MessageShowHelper.ShowInfoMessage("This file is not in locked!");
        }

        foreach (var item in rM_PROCESS_INFOs)
        {
            if (item.Process.dwProcessId == 0)
            {
                continue;
            }
            FileInUseVM fIleInUseVM = new FileInUseVM();
            fIleInUseVM.LockFileName = item.strAppName;
            fIleInUseVM.LockProcessID = item.Process.dwProcessId;

            fIleInUseVM.ProcessStartTime = item.Process.ProcessStartTime.ToDateTime();
            fIleInUseVM.RefreshAction += RefreshAction;

            ProcessList.Add(fIleInUseVM);
        }
        RmEndSession(pSessionHandel);
    }

    private void RefreshAction(FileInUseVM vm)
    {
        ProcessList.Remove(vm);
    }
}

public partial class FileInUseVM : BaseVM
{
    [ObservableProperty]
    private string lockFileName;

    [ObservableProperty]
    private uint lockProcessID;

    [ObservableProperty]
    private DateTime processStartTime;

    public Action<FileInUseVM> RefreshAction;

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetProcessTimes(IntPtr hProcess, out FILETIME lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

    [RelayCommand]
    private void KillProcess()
    {
        //判断平台
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        try
        {
            var thisproc = Process.GetProcessById((int)LockProcessID);

            bool res = GetProcessTimes(thisproc.Handle, out FILETIME lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);
            if (res)
            {
                if (lpCreationTime.ToDateTime() == ProcessStartTime)
                {
                    ProcessHelper.KillProcessByID((int)LockProcessID);
                    RefreshAction?.Invoke(this);
                }
            }
        }
        catch (Exception ex)
        {
            MessageShowHelper.ShowErrorMessage(ex.Message);
        }
    }
}