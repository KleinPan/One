using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace One.Control.Controls.CirclePanel;

public class DrawPrizeIndexToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var num = System.Convert.ToInt32(value);
        //var brush = (SolidColorBrush)Application.Current.TryFindResource("DrawPrizeSingularSolidColorBrush");
        //if (num % 2 == 1)
        //    brush = (SolidColorBrush)Application.Current.TryFindResource("DrawPrizeDualSolidColorBrush");
        //return brush;

        var brush = new System.Windows.Media.SolidColorBrush(Color.FromRgb(237, 90, 101));//红色
        if (num % 2 == 1)
            brush = new System.Windows.Media.SolidColorBrush(Color.FromRgb(192, 72, 81));
        return brush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}