// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.Network;
using One.Toolbox.Views.Pages;
using One.Toolbox.Views.Serialport;
using One.Toolbox.Views.Settings;

using System.Collections.ObjectModel;

namespace One.Toolbox.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _applicationTitle = String.Empty;

    [ObservableProperty]
    private ObservableCollection<MainMenuItemViewModel> _navigationItems = new();

    //[ObservableProperty]
    //private ObservableCollection<MenuItem> _trayMenuItems = new();

    [ObservableProperty]
    private MainMenuItemViewModel currentMenuItem;

    public MainWindowViewModel()
    {
        InitializeViewModel();

        ConfigHelper.Instance.Load();
        isInitialized = true;
    }

    //public MainWindowViewModel(INavigationService navigationService)
    //{
    //    if (!isInitialized)
    //        InitializeViewModel();
    //}

    private new void InitializeViewModel()
    {
        ApplicationTitle = "One.Toolbox";


        
        NavigationItems = new ObservableCollection<MainMenuItemViewModel>();

        //https://www.xicons.org/#/
        //https://pictogrammers.com/library/mdi/
        NavigationItems.Add(new MainMenuItemViewModel()
        {
            Header = "Home",
            //Icon = FontAwesome.Sharp.IconChar.Home,
            Icon = ResourceHelper.Dic["HomeRound"],
            TargetPageType = typeof(Views.Pages.DashboardPage),
            Content = new DashboardPage(),
        }) ;

        NavigationItems.Add(new MainMenuItemViewModel()
        {
            Header = "Text",
            Icon = ResourceHelper.Dic["TextFieldsRound"],
            TargetPageType = typeof(Views.Pages.StringConvertPage),
            Content = new StringConvertPage(),
        });
        NavigationItems.Add(new MainMenuItemViewModel()
        {
            Header = "Com",
            Icon = ResourceHelper.Dic["SerialPort20Regular"],
            TargetPageType = typeof(Views.Serialport.SerialportPage),
            Content = new SerialportPage(),
        });
        NavigationItems.Add(new MainMenuItemViewModel()
        {
            Header = "Net",
            Icon = ResourceHelper.Dic["Network"],
            TargetPageType = typeof(Views.Network.NetworklPage),
            Content = new NetworklPage(),
        });

        NavigationItems.Add(new MainMenuItemViewModel()
        {
            Header = "Setting",
            Icon = ResourceHelper.Dic["SettingsRound"],
            TargetPageType = typeof(Views.Settings.SettingsPage),
            Content = new SettingsPage(),
            Dock = System.Windows.Controls.Dock.Bottom,
        });

        CurrentMenuItem = NavigationItems.First();
    }

    #region 框架逻辑

    partial void OnCurrentMenuItemChanged(MainMenuItemViewModel? oldValue, MainMenuItemViewModel newValue)
    {
        if (oldValue != null)
        {
            var vm = oldValue.Content.DataContext as BaseViewModel;
            vm.OnNavigatedLeave();
        }
        var vmNew = newValue.Content.DataContext as BaseViewModel;
        vmNew.OnNavigatedEnter();
    }

    #endregion 框架逻辑
}