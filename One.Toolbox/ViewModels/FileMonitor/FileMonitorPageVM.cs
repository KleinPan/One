using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using One.Toolbox.ExtensionMethods;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;
using Vanara.PInvoke;
using static Vanara.PInvoke.RstrtMgr;

namespace One.Toolbox.ViewModels.FileMonitor;

public partial class FileMonitorPageVM : BaseVM
{
    public FileMonitorPageVM() { }

    public override void OnNavigatedEnter()
    {
        base.OnNavigatedEnter();
        InitData();
    }

    void InitData() { }

    public ObservableCollection<FIleInUseVM> FIleInUseVMs { get; set; } =
        new ObservableCollection<FIleInUseVM>();

    private void TestFile(string filePath)
    {

        FIleInUseVMs.Clear();
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
            var fIleInUseVM = new FIleInUseVM();
            fIleInUseVM.LockFileName = item.strAppName;
            fIleInUseVM.LockProcessID = item.Process.dwProcessId;
            FIleInUseVMs.Add( fIleInUseVM );
        }
    }
}

public partial class FIleInUseVM : BaseVM
{
    [ObservableProperty]
    private string lockFileName;

    [ObservableProperty]
    private uint lockProcessID;
}
