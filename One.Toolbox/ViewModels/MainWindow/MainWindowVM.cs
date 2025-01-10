using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Dashboard;
using One.Toolbox.Views.DataProcess;
using One.Toolbox.Views.Network;
using One.Toolbox.Views.Serialport;
using One.Toolbox.Views.Settings;

using System.Collections.ObjectModel;

using WindowHelper = One.Toolbox.Helpers.WindowHelper;

namespace One.Toolbox.ViewModels.MainWindow;

public partial class MainWindowVM : BaseVM
{
    [ObservableProperty]
    private string _applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<MainMenuItemVM> _navigationItems = new();

    //[ObservableProperty]
    //private ObservableCollection<MenuItem> _trayMenuItems = new();

    [ObservableProperty]
    private MainMenuItemVM currentMenuItem;

    public MainWindowVM()
    {
        InitializeViewModel();

        //ConfigHelper.Instance.LoadLocalDefaultSetting();
    }

    private new void InitializeViewModel()
    {
        ApplicationTitle = "One.Toolbox";

        NavigationItems = new ObservableCollection<MainMenuItemVM>
        {
            //https://www.xicons.org/#/
            //https://pictogrammers.com/library/mdi/
            new MainMenuItemVM()
            {
                Header = "Home",
                //Icon = FontAwesome.Sharp.IconChar.Home,
                Icon = ResourceHelper.Dic["HomeRound"],
                //TargetPageType = typeof(Views.Pages.DashboardPage),
                Content = new DashboardPage(),
            },
            new MainMenuItemVM()
            {
                Header = "Text",
                Icon = ResourceHelper.Dic["TextFieldsRound"],
                //TargetPageType = typeof(Views.Pages.StringConvertPage),
                Content = new StringConvertPage(),
            },
            new MainMenuItemVM()
            {
                Header = "Com",
                Icon = ResourceHelper.Dic["SerialPort24Filled"],
                //TargetPageType = typeof(Views.Serialport.SerialportPage),
                Content = new SerialportPage(),
            },
            new MainMenuItemVM()
            {
                Header = "Net",
                Icon = ResourceHelper.Dic["Network"],
                //TargetPageType = typeof(Views.Network.NetworklPage),
                Content = new NetworklPage(),
            },

            new MainMenuItemVM()
            {
                Header = "Stick",
                Icon = ResourceHelper.Dic["Note24Regular"],
                //TargetPageType = typeof(Views.NotePad.NotePadPage),
                Content = new Views.Stick.StickPage(),
            },
            new()
            {
                Header = "EveryImage",
                Icon = ResourceHelper.Dic["ImageArea"],
                //TargetPageType = typeof(Views.NotePad.NotePadPage),
                Content = new Views.BingImage.BingImagePage(),
            },
             new()
            {
                Header = "NetSpeed",
                Icon = ResourceHelper.Dic["TopSpeed20Regular"],
                //TargetPageType = typeof(Views.NotePad.NotePadPage),
                Content = new Views.NetSpeed.NetSpeedPage(),
            },
            new()
            {
                Header = "FileMonitor",
                Icon = ResourceHelper.Dic["File"],
                //TargetPageType = typeof(Views.NotePad.NotePadPage),
                Content = new Views.FileMonitor.FileMonitorPage(),
            },
            //NavigationItems.Add(new MainMenuItemViewModel()
            //{
            //    Header = "LotteryDraw",
            //    Icon = ResourceHelper.Dic["IncompleteCircleFilled"],
            //    Content = new Views.LotteryDraw.LotteryDrawPage(),
            //});

            //new MainMenuItemVM()
            //{
            //    Header = "Test",
            //    Icon = ResourceHelper.Dic["IncompleteCircleFilled"],
            //    Content = new Views.TestPage(),
            //},
        //倒叙
        new()
            {
                Header = "Setting",
                Icon = ResourceHelper.Dic["SettingsRound"],
                //TargetPageType = typeof(Views.Settings.SettingsPage),
                Content = new SettingsPage(),
                Dock = System.Windows.Controls.Dock.Bottom,
            },
            new MainMenuItemVM()
            {
                Header = "CloudSetting",
                Icon = ResourceHelper.Dic["CloudSyncFilled"],
                //TargetPageType = typeof(Views.Settings.SettingsPage),
                Content = new CloudSettingsPage(),
                Dock = System.Windows.Controls.Dock.Bottom,
            }
        };
        CurrentMenuItem = NavigationItems.First();
        base.InitializeViewModel();
    }

    [RelayCommand]
    private static void PushMainWindow2Top()
    {
        if (Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility != Visibility.Visible)
        {
            Application.Current.MainWindow.Show();
            WindowHelper.SetWindowToForeground(Application.Current.MainWindow);
        }
    }

    [RelayCommand]
    private static void ExitApp()
    {
        Application.Current.Shutdown();
    }

    #region 框架逻辑

    partial void OnCurrentMenuItemChanged(MainMenuItemVM? oldValue, MainMenuItemVM newValue)
    {
        if (oldValue != null)
        {
            var vm = oldValue.Content.DataContext as BaseVM;
            vm.OnNavigatedLeave();
        }
        if (newValue != null)
        {
            var vmNew = newValue.Content.DataContext as BaseVM;
            vmNew.OnNavigatedEnter();
        }
    }

    #endregion 框架逻辑
}