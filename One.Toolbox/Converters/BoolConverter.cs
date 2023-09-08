using System.Globalization;
using System.Windows.Data;

namespace One.Toolbox.Converters
{
    /// <summary> bool为true时显示连接，否则显示断开 </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    public class boolConnected : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Disconnect" : "Connect";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }



    /// <summary> bool为true时显示连接，否则显示断开 </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }

}