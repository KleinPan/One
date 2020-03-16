using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace One.Control.Converters
{
    public class BoolToColorConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = (bool)value;

            if (status)
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromRgb(252, 57, 90));//红色

                //return Color.FromArgb(254,193,7,1);
            }
            else
            {
                return new System.Windows.Media.SolidColorBrush(Color.FromRgb(112, 255, 1));//绿色

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
