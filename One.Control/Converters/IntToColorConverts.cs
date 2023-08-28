using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace One.Control.Converters
{
    public class IntToColorConverts : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int status = (int)value;

            if (status == 1)
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromRgb(252, 57, 90));//红色
            }
            else if (status == 0)
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromRgb(112, 255, 1));//绿色
            }
            else
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromRgb(252, 57, 90));//红色
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}