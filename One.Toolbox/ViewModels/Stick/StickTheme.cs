using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace One.Toolbox.ViewModels.Stick;

public partial class StickTheme : ObservableObject
{
    [ObservableProperty]
    private System.Windows.Media.Brush headerBrush;

    [ObservableProperty]
    private System.Windows.Media.Brush backBrush;

    public StickTheme()
    {
    }

    public StickTheme(string head, string back)
    {
        HeaderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(head));
        BackBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(back));
    }
}