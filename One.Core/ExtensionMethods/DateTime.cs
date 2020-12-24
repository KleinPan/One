﻿using System;

namespace One.Core.ExtensionMethods
{
    public static class ExtensionMethodsForDateTime
    {
        /// <summary> 获取当天起始时间 00:00:00:000 </summary>
        /// <param name="this"> The @this to act on. </param>
        /// <returns> A DateTime of the day with the time set to "00:00:00:000". </returns>
        public static DateTime StartOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day);
        }

        /// <summary> 获取当天最后一刻 "23:59:59:999" The last moment of the day. Use "DateTime2" column type in sql to keep the precision. </summary>
        /// <param name="this"> The @this to act on. </param>
        /// <returns> A DateTime of the day with the time set to "23:59:59:999". </returns>
        public static DateTime EndOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }
    }
}