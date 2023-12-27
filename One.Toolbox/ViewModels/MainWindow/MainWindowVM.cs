﻿using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Dashboard;
using One.Toolbox.Views.DataProcess;
using One.Toolbox.Views.Network;
using One.Toolbox.Views.Serialport;
using One.Toolbox.Views.Settings;

using System.Collections.ObjectModel;

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
        isInitialized = true;
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
                Header = "NotePad",
                Icon = ResourceHelper.Dic["EditNoteFilled"],
                //TargetPageType = typeof(Views.NotePad.NotePadPage),
                Content = new Views.NotePad.NotePadPage(),
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