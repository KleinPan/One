// This Source Code Form is subject to the terms of the MIT License. If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT. Copyright (C) Leszek Pomianowski and WPF UI Contributors. All Rights Reserved.

using One.Toolbox.Helpers;

using System.Diagnostics;
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
        //string aa = @"D:\Program Files\Oray\SunLogin\SunloginClient\SunloginClient.exe";
        //Process.Start(aa);
        //HardwareHelper.SearchPortByLocation(DeviceType.ComPort, "PCIROOT(0)#PCI(1400)#USBROOT(0)#USB(10)", x => x.Count > 2);
    }
}