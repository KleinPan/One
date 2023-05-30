using One.Toolbox.Enums;

using System.Globalization;
using System.Windows.Data;

namespace One.Toolbox.Converters
{
    public class CommonProtoclShowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (CommunProtocalType)value;

            string type2 = parameter.ToString();
            var type3 = (CommunProtocalType)Enum.Parse(typeof(CommunProtocalType), type2);

            if (type == type3)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //bool source = System.Convert.ToBoolean(value);
            //int para = System.Convert.ToInt32(parameter);

            //if (source == true)
            //{
            //    return para;

            //}
            //else
            //{
            //    return null;
            //}

            throw new NotImplementedException();
        }
    }
}