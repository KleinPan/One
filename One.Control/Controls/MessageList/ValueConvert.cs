using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace One.Controls.Controls.MessageList
{
    /// <summary>宽度减少10</summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class ConverterWidth : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if ((double)value > 30)
            {
                var temp = (double)value - 27;//滚动条宽度
                return temp;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }




        }

        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string str = value as string;
            //DateTime txtDate;
            //if (DateTime.TryParse(str, out txtDate))
            //{
            //    return txtDate;
            //}
            //return DependencyProperty.UnsetValue;
            return value;
        }
    }
}