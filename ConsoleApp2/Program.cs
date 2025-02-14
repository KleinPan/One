using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace ConsoleApp2
{
    internal class Program
    {
        public static ObservableCollection<NetworkInterface> NetSpeedItems { get; set; } = new  ();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var item in interfaces)
            {
                if (!item.OperationalStatus.Equals(OperationalStatus.Up) || item.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                {
                  
                    continue;
                }

                NetSpeedItems.Add(item);
            }
        }
    }
}
