// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.Helpers;

using System.ServiceProcess;

namespace One.Toolbox.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;

    public DashboardViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void Test()
    {
        HardwareHelper.SearchPortByLocation(DeviceType.ComPort, "PCIROOT(0)#PCI(1400)#USBROOT(0)#USB(10)");
    }
}