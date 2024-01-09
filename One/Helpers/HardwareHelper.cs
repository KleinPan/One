using System.Runtime.InteropServices;
using System.Threading;

using Vanara.PInvoke;

using static Vanara.PInvoke.SetupAPI;

namespace One.Toolbox.Helpers
{
    public enum DeviceType
    {
        ComPort,
        Android,
        Modem,
        USBDevice,
    }

    public class DeviceInfo
    {
        public int ComPort { get; set; }
        public string Description { get; set; }

        /// <summary> 设备路径 </summary>
        public string DevicePath { get; set; }

        public string FriendlyName { get; set; }
        public string LoctionPath { get; set; }
    }

    internal class HardwareHelper
    {
        private static Mutex m_mutex = new Mutex();

        public static List<DeviceInfo> GetAllPluginDevice(DeviceType deviceType)
        {
            m_mutex.WaitOne();
            Guid _NewGuid = Guid.Parse("{86e0d1e0-8089-11d0-9ce4-08003e301f73}");

            switch (deviceType)
            {
                case DeviceType.ComPort:

                    //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-comport
                    _NewGuid = Guid.Parse("{86e0d1e0-8089-11d0-9ce4-08003e301f73}");
                    break;

                case DeviceType.Android://用后边那个

                    var ClassGuid = Guid.Parse("{3F966BD9-FA04-4ec5-991C-D326973B5128}");//AndroidUsbDeviceClass ClassGuid

                    //https://android.googlesource.com/platform/development/+/refs/heads/master/host/windows/usb/android_winusb.inf
                    _NewGuid = Guid.Parse("{f72fe0d4-cbcb-407d-8814-9ed673d0dd6b}");//Dev_AddReg DeviceInterfaceGUIDs

                    break;

                case DeviceType.Modem:

                    //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-modem
                    _NewGuid = Guid.Parse("{2C7089AA-2E0E-11D1-B114-00C04FC2AAE4}");
                    break;

                case DeviceType.USBDevice:

                    //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-device
                    _NewGuid = Guid.Parse("{a5dcbf10-6530-11d2-901f-00c04fb951ed}");  //USB_DEVICE
                    break;

                default:
                    break;
            }

            var devInfoSetHandle = Vanara.PInvoke.SetupAPI.SetupDiGetClassDevs(_NewGuid, null, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);

            var interfaces = Vanara.PInvoke.SetupAPI.SetupDiEnumDeviceInterfaces(devInfoSetHandle, _NewGuid);

            List<DeviceInfo> deviceInfos = new List<DeviceInfo>();

            foreach (var item in interfaces)
            {
                DeviceInfo deviceInfo = new DeviceInfo();
                SetupDiGetDeviceInterfaceDetail(devInfoSetHandle, item, out string path, out SP_DEVINFO_DATA deviceInfoData);
                deviceInfo.DevicePath = path;

                IntPtr name = IntPtr.Zero;
                uint bufferSize = 1024;
                name = Marshal.AllocHGlobal((int)bufferSize);
                string tempString;
                REG_VALUE_TYPE propertyRegDataType;
                uint requiredSize;

                var temp1 = SetupDiGetDeviceRegistryProperty(devInfoSetHandle, deviceInfoData, SPDRP.SPDRP_DEVICEDESC, out propertyRegDataType, name, bufferSize, out requiredSize);

                if (temp1)
                {
                    tempString = Marshal.PtrToStringAuto(name);
                    deviceInfo.Description = tempString;
                    tempString = "";
                }

                var temp2 = SetupDiGetDeviceRegistryProperty(devInfoSetHandle, deviceInfoData, SPDRP.SPDRP_FRIENDLYNAME, out propertyRegDataType, name, bufferSize, out requiredSize);
                if (temp2)
                {
                    tempString = Marshal.PtrToStringAuto(name);
                    deviceInfo.FriendlyName = tempString;
                    tempString = "";
                }

                var temp3 = SetupDiGetDeviceRegistryProperty(devInfoSetHandle, deviceInfoData, SPDRP.SPDRP_LOCATION_PATHS, out propertyRegDataType, name, bufferSize, out requiredSize);
                if (temp3)
                {
                    tempString = Marshal.PtrToStringAuto(name);
                    deviceInfo.LoctionPath = tempString;
                    tempString = "";
                }
                else
                {
                    var error = Vanara.PInvoke.Win32Error.GetLastError();
                }

                if (deviceType == DeviceType.ComPort)
                {
                    var hDevKey = SetupDiOpenDevRegKey(devInfoSetHandle, deviceInfoData, DICS_FLAG.DICS_FLAG_GLOBAL, 0, DIREG.DIREG_DEV, System.Security.AccessControl.RegistryRights.ExecuteKey);

                    uint dataLen = 256;
                    IntPtr lpData = IntPtr.Zero;
                    lpData = Marshal.AllocHGlobal((int)dataLen);
                    var RegQueryValueExres = Vanara.PInvoke.AdvApi32.RegQueryValueEx(hDevKey, "PortName", IntPtr.Zero, out REG_VALUE_TYPE lpType, lpData, ref dataLen);

                    Vanara.PInvoke.AdvApi32.RegCloseKey(hDevKey);

                    var portName = Marshal.PtrToStringAuto(lpData);
                    int portNum = int.Parse(portName.Replace("COM", ""));
                    deviceInfo.ComPort = portNum;
                }

                deviceInfos.Add(deviceInfo);
            }

            SetupDiDestroyDeviceInfoList(devInfoSetHandle);

            m_mutex.ReleaseMutex();
            return deviceInfos;
        }

        /// <summary> 搜索端口 </summary>
        /// <param name="deviceType"> </param>
        /// <param name="location">   </param>
        /// <param name="timeout">    </param>
        /// <returns> </returns>
        /// <exception cref="Exception"> </exception>
        public static List<DeviceInfo> SearchPortByLocation(DeviceType deviceType, string location, Predicate<List<DeviceInfo>> predicate, int timeout = 60)
        {
            List<DeviceInfo> aa = new List<DeviceInfo>();
            for (int i = 0; i < timeout; i++)
            {
                var list = GetAllPluginDevice(deviceType);

                foreach (var deviceInfo in list)
                {
                    if (deviceInfo.LoctionPath.StartsWith(location))
                    {
                        aa.Add(deviceInfo);
                    }
                }

                if (predicate(aa))
                {
                    return aa;
                }

                Thread.Sleep(1000);
            }

            throw new Exception("Not finf target device!");
        }
    }
}