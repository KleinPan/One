using System.IO;
using System.Runtime.InteropServices;

using Vanara.PInvoke;

using Windows.Win32;

namespace One.Toolbox.ViewModels.BingImage;

public partial class UsefullImageInfoVM : ObservableObject
{
    /// <summary> </summary>
    /// <param name="uAction">  指定要设置的参数。参考uAction常数表。 </param>
    /// <param name="uParam">   参考uAction常数表。 </param>
    /// <param name="lpvParam"> 按引用调用的Integer、Long和数据结构。 </param>
    /// <param name="fuWinIni"> 这个参数规定了在设置系统参数的时候，是否应更新用户设置参数。 </param>
    /// <returns> </returns>
    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern int SystemParametersInfo(
                int uAction,
                int uParam,
                string lpvParam,
                int fuWinIni
            );

    public static void SetImageToDesktop(string filePath)
    {
        SystemParametersInfo(20, 1, filePath, 1);

        /*
        IntPtr a = Marshal.StringToHGlobalAnsi(filePath);
        unsafe
        {
            //https://github.com/microsoft/CsWin32
            PInvoke.SystemParametersInfo(Windows.Win32.UI.WindowsAndMessaging.SYSTEM_PARAMETERS_INFO_ACTION.SPI_SETDESKWALLPAPER, 1, &a, Windows.Win32.UI.WindowsAndMessaging.SYSTEM_PARAMETERS_INFO_UPDATE_FLAGS.SPIF_UPDATEINIFILE);
        }
        */
    }

    [ObservableProperty]
    private string downloadUrl;

    [ObservableProperty]
    private string copyright;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string localImageName;

    [ObservableProperty]
    private string localImagePath;

    [RelayCommand]
    private void Set()
    {
        SetImageToDesktop(LocalImagePath);
    }

    public UsefullImageInfoModel ToModel()
    {
        UsefullImageInfoModel usefullImageInfoViewModel = new UsefullImageInfoModel();
        usefullImageInfoViewModel.Title = Title;
        usefullImageInfoViewModel.Copyright = Copyright;
        usefullImageInfoViewModel.LocalImagePath = LocalImagePath;
        usefullImageInfoViewModel.LocalImageName = LocalImageName;
        usefullImageInfoViewModel.DownloadUrl = DownloadUrl;

        return usefullImageInfoViewModel;
    }
}

public class UsefullImageInfoModel
{
    public string DownloadUrl;

    public string Copyright;

    public string Title;

    public string LocalImageName;

    public string LocalImagePath;

    public UsefullImageInfoVM ToVM()
    {
        UsefullImageInfoVM usefullImageInfoViewModel = new UsefullImageInfoVM();
        usefullImageInfoViewModel.Title = Title;
        usefullImageInfoViewModel.Copyright = Copyright;
        usefullImageInfoViewModel.LocalImagePath = LocalImagePath;
        usefullImageInfoViewModel.LocalImageName = LocalImageName;
        usefullImageInfoViewModel.DownloadUrl = DownloadUrl;

        return usefullImageInfoViewModel;
    }
}