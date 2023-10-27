using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace One.Core.Helpers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short Year;
        public short Month;
        public short DayOfWeek;
        public short Day;
        public short Hour;
        public short Minute;
        public short Second;
        public short Millisecond;
    }

    public class TimeHelper
    {
        [DllImportAttribute("Kernel32.dll")]
        private static extern void GetLocalTime(SYSTEMTIME st);

        [DllImportAttribute("Kernel32.dll")]
        private static extern void SetLocalTime(SYSTEMTIME st);

        public static void SetSystemTime(DateTime dateTime)
        {
            SYSTEMTIME MySystemTime = new SYSTEMTIME();

            //GetLocalTime(MySystemTime);

            MySystemTime.Year = (short)dateTime.Year;

            MySystemTime.Month = (short)dateTime.Month;

            MySystemTime.Day = (short)dateTime.Day;
            MySystemTime.DayOfWeek = (short)dateTime.DayOfWeek;

            MySystemTime.Hour = (short)(dateTime.Hour);

            MySystemTime.Minute = (short)dateTime.Minute;

            MySystemTime.Second = (short)dateTime.Second;
            MySystemTime.Millisecond = (short)dateTime.Millisecond;

            try
            {
                SetLocalTime(MySystemTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}