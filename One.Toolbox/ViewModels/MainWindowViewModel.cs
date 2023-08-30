// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.Helpers;

using System.Collections.ObjectModel;

namespace One.Toolbox.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<object> _navigationFooter = new();

    [ObservableProperty]
    private ObservableCollection<object> _navigationItems = new();

    //[ObservableProperty]
    //private ObservableCollection<MenuItem> _trayMenuItems = new();

    public MainWindowViewModel()
    {
        
    }
    //public MainWindowViewModel(INavigationService navigationService)
    //{
    //    if (!isInitialized)
    //        InitializeViewModel();
    //}

    private new void InitializeViewModel()
    {
        ApplicationTitle = "One.Toolbox";

        NavigationItems = new ObservableCollection<object>
        {
            //new NavigationViewItem()
            //{
            //    Content = "Home",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.Home16 },
            //    TargetPageType = typeof(Views.Pages.DashboardPage)
            //},

            //new NavigationViewItem()
            //{
            //   Content = "Text",
            //   Icon = new SymbolIcon { Symbol = SymbolRegular. TextNumberFormat20},
            //   TargetPageType = typeof(Views.Pages.StringConvertPage),
            //},
            //new NavigationViewItem()
            //    {
            //        Content = "Com",
            //        Icon = new SymbolIcon { Symbol = SymbolRegular.SerialPort16 },
            //        TargetPageType = typeof(Views.Serialport.SerialportPage),
            //    },
            //new NavigationViewItem()
            // {
            //    Content = "Net",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.NetworkCheck20},
            //    TargetPageType = typeof(Views.Pages.NetworklPage),
            // },
        };

        NavigationFooter = new ObservableCollection<object>
            {
                //new NavigationViewItem()
                //{
                //    Content = "Settings",
                //    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                //    TargetPageType = typeof(Views.Settings.SettingsPage)
                //}
            };

        //win11 有问题
        //TrayMenuItems = new ObservableCollection<MenuItem>
        //    {
        //        new MenuItem
        //        {
        //            Header = "Home",
        //            Tag = "tray_home"
        //        }
        //    };

        ConfigHelper.Instance.Load();
        isInitialized = true;
    }
}