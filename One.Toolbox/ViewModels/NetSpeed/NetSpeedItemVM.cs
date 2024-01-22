using One.Toolbox.ViewModels.Base;

using System.Net.NetworkInformation;

namespace One.Toolbox.ViewModels.NetSpeed;

public partial class NetSpeedItemVM : BaseVM
{
    private readonly NetworkInterface adapter;
    private DateTime lastUpdate;

    [ObservableProperty]
    private string speedSentHuman;

    [ObservableProperty]
    private string speedReceivedHuman;

    public string InterfaceName { get; set; }
    public string PhysicalAddress { get; private set; }
    private long LastBytesSent { get; set; }
    private long LastBytesReceived { get; set; }

    private int UpdateInterval { get; set; }

    public Action<NetSpeedEventArgs> SpeedAction;

    public NetSpeedItemVM(NetworkInterface networkInterface, int updateInterval = 1000)
    {
        adapter = networkInterface;
        UpdateInterval = updateInterval;

        InterfaceName = adapter.Name;
        var macAddress = networkInterface.GetPhysicalAddress();

        PhysicalAddress = BitConverter.ToString(macAddress.GetAddressBytes()).Replace("-", ":");
    }

    public void Start(CancellationToken cancellationToken = default)
    {
        lastUpdate = DateTime.Now;
        _ = Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    UpdateSpeed();
                    await Task.Delay(UpdateInterval);
                }
                catch (TaskCanceledException)
                {
                    WriteDebugLog($"Task canceled.");

                    break;
                }
                catch (Exception e)
                {
                    WriteDebugLog(e.ToString() + "Failed to update net speed.");
                }
            }
        });
    }

    public override string ToString()
    {
        return $"{InterfaceName}({PhysicalAddress})";
    }

    public bool First { get; set; } = true;

    private void UpdateSpeed()
    {
        // Check if the interface is up （Up表示 网络接口已打开；它可以传输数据包。）
        if (!adapter.OperationalStatus.Equals(OperationalStatus.Up))
        {
            WriteDebugLog($"Net interface {ToString()} is {adapter.OperationalStatus}");
            return;
        }

        // Get the current bytes sent and received
        var bytesSent = adapter.GetIPStatistics().BytesSent;
        var bytesReceived = adapter.GetIPStatistics().BytesReceived;

        //Debug.WriteLine($"BytesSent: {bytesSent}, BytesReceived: {bytesReceived}");

        // Calculate the speed

        var speedSent = (bytesSent - LastBytesSent) / (DateTime.Now - lastUpdate).TotalSeconds;
        var speedReceived = (bytesReceived - LastBytesReceived) / (DateTime.Now - lastUpdate).TotalSeconds;

        // Update the last update time
        lastUpdate = DateTime.Now;

        // Update the last bytes sent and received
        LastBytesSent = bytesSent;
        LastBytesReceived = bytesReceived;

        if (First)
        {
            First = false;
            return;
        }

        SpeedAction?.Invoke(new NetSpeedEventArgs(Math.Round(speedSent, 2), Math.Round(speedReceived, 2)));
        SpeedSentHuman = HumanReadableSpeed(speedSent);
        SpeedReceivedHuman = HumanReadableSpeed(speedReceived);

        //Debug.WriteLine($"Sent: {speedSent}, Received: {speedReceived}");

        //Debug.WriteLine($"SentHuman: {SpeedSentHuman}, ReceivedHuman: {SpeedReceivedHuman}");
    }

    public static string HumanReadableSpeed(double bytesPerSecond)
    {
        if (bytesPerSecond < 1024)
        {
            return $"{bytesPerSecond:0.00} B/s";
        }
        else if (bytesPerSecond < 1024 * 1024)
        {
            return $"{(bytesPerSecond / 1024.0):0.00} KB/s";
        }
        else if (bytesPerSecond < 1024 * 1024 * 1024)
        {
            return $"{(bytesPerSecond / 1024.0 / 1024.0):0.00} MB/s";
        }
        else
        {
            return $"{(bytesPerSecond / 1024.0 / 1024.0 / 1024.0):0.00} GB/s";
        }
    }
}

public class NetSpeedEventArgs
{
    public double SpeedSent { get; set; }
    public double SpeedReceived { get; set; }

    public NetSpeedEventArgs(double speedSent, double speedReceived)
    {
        SpeedSent = speedSent;
        SpeedReceived = speedReceived;
    }
}