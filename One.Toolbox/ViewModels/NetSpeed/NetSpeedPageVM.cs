using Microsoft.Extensions.DependencyInjection;

using One.Core.ExtensionMethods;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;

using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows.Controls;

namespace One.Toolbox.ViewModels.NetSpeed;

public partial class NetSpeedPageVM : BaseVM
{
    [ObservableProperty]
    private NetSpeedItemVM netSpeedSelectItemVM;

    public ObservableCollection<NetSpeedItemVM> NetSpeedItems { get; set; } = new ObservableCollection<NetSpeedItemVM>();

    private CancellationTokenSource cts = new CancellationTokenSource();

    public NetSpeedPlotVM NetSpeedPlot { get; set; } = new NetSpeedPlotVM();

    public NetSpeedPageVM()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        NetSpeedItems.AddRange(interfaces.Select(i => new NetSpeedItemVM(i)).ToList());

        NetSpeedItems.ForEach(i => i.Start(cts.Token));
    }

    public string LastAdapterName { get; set; }

    public override void InitializeViewModel()
    {
        LoadSetting();
        NetSpeedSelectItemVM = NetSpeedItems.FirstOrDefault(i => i.InterfaceName == LastAdapterName);

        base.InitializeViewModel();
    }

    public override void OnNavigatedLeave()
    {
        base.OnNavigatedLeave();

        LastAdapterName = NetSpeedSelectItemVM.InterfaceName;

        SaveSetting();
    }

    private void LoadSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();
        LastAdapterName = service.AllConfig.NetSpeedSetting.LastAdapterName;
    }

    private void SaveSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();
        service.AllConfig.NetSpeedSetting.LastAdapterName = LastAdapterName;
        service.Save();
    }

    [RelayCommand]
    private void SelectedNetSpeedItemChanged(SelectionChangedEventArgs obj)
    {
        if (obj.RemovedItems.Count > 0)
        {
            var removedItem = obj.RemovedItems[0] as NetSpeedItemVM;
            removedItem.SpeedAction -= NetSpeedPlot.OnSpeedChange;
        }

        if (obj.AddedItems[0] is NetSpeedItemVM addItem)
        {
            addItem.SpeedAction += NetSpeedPlot.OnSpeedChange;
            //addItem.Start(cts.Token);
        }
    }
}