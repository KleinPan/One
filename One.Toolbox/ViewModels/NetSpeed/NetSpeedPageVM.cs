using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using One.Base.ExtensionMethods;
using One.Toolbox.Messenger;
using One.Toolbox.Services;
using One.Toolbox.ViewModels.Base;
using One.Toolbox.Views.NetSpeed;

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

    [ObservableProperty]
    private bool showSmallWnd;

    public NetSpeedPageVM()
    {
        WeakReferenceMessenger.Default.Register<CloseMessage>(this, (r, m) =>
        {
            // Handle the message here, with r being the recipient and m being the input message.
            // Using the recipient passed as input makes it so that the lambda expression doesn't
            // capture "this", improving performance.

            SaveSetting();
        });

        var interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var item in interfaces)
        {
            
            if (!item.OperationalStatus.Equals(OperationalStatus.Up) || item.NetworkInterfaceType == NetworkInterfaceType.Loopback|| item.Speed==-1)
            {
                WriteDebugLog($"Net interface {ToString()} is {item.OperationalStatus}");
                continue;
            }

            NetSpeedItems.Add(new NetSpeedItemVM(item));
        }

        NetSpeedItems.ForEach(i => i.Start(cts.Token));

        LoadSetting();
        NetSpeedSelectItemVM = NetSpeedItems.FirstOrDefault(i => i.InterfaceName == LastAdapterName);
        if (NetSpeedSelectItemVM == null)
        {
            NetSpeedSelectItemVM = NetSpeedItems.FirstOrDefault();
        }
        NetSpeedSelectItemVM.SpeedAction += NetSpeedPlot.OnSpeedChange;

        if (ShowSmallWnd)
        {
            NetSpeedWnd.DataContext = NetSpeedSelectItemVM;

            NetSpeedWnd.Show();
        }
    }

    public string LastAdapterName { get; set; }

    private void LoadSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();
        LastAdapterName = service.AllConfig.NetSpeedSetting.LastAdapterName;
        ShowSmallWnd = service.AllConfig.NetSpeedSetting.ShowSpeedWndDefault;
    }

    private void SaveSetting()
    {
        var service = App.Current.Services.GetService<SettingService>();
        service.AllConfig.NetSpeedSetting.LastAdapterName = LastAdapterName = NetSpeedSelectItemVM.InterfaceName;
        service.AllConfig.NetSpeedSetting.ShowSpeedWndDefault = ShowSmallWnd;
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

    private NetSpeedWnd NetSpeedWnd = new();

    partial void OnShowSmallWndChanged(bool value)
    {
        if (value)
        {
            NetSpeedWnd.DataContext = NetSpeedSelectItemVM;
            NetSpeedWnd.Show();
        }
        else
        {
            NetSpeedWnd.Hide();
        }
    }
}