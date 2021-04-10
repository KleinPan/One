using System;

namespace One.Core.Helper
{
    public class DatetimeHelper
    {
        /// <summary> 格式化时间戳，生成例如 02小时03分钟04秒的格式 </summary>
        /// <param name="ts"> </param>
        /// <returns> </returns>
        private static string TimeSpanFormat(TimeSpan ts)
        {
            string str = "";
            if (ts.Hours > 0)

            {
                str = ts.Hours.ToString("00") + "小时 " + ts.Minutes.ToString("00") + "分 " + ts.Seconds.ToString("00") + "秒";
            }

            if (ts.Hours == 0 && ts.Minutes > 0)

            {
                str = ts.Minutes.ToString("00") + "分 " + ts.Seconds + "秒";
            }

            if (ts.Hours == 0 && ts.Minutes == 0)

            {
                str = ts.Seconds.ToString("00") + "秒";
            }

            return str;
        }


        /// <summary>
        /// DateTime --> long
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ConvertDataTimeToLong(DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
            return timeStamp;
        }


        /// <summary>
        /// long --> DateTime
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvertLongToDateTime(long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
    }
}