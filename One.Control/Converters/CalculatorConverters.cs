using System;
using System.Globalization;
using System.Windows.Data;

namespace One.Control.Converters
{
    public class CalculatorConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double original = (double)value;
            bool can = double.TryParse(parameter.ToString(), out double add);
            if (can)
            {
                return original + add;
            }
            else
            {
                return original;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}