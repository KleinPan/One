using System.Windows.Media;

namespace One.Toolbox.ViewModels.LotteryDraw;

public class LotteryDrawModel
{
    public int StartAngle { get; set; }
    public int Angle { get; set; }
    public string Title { get; set; }
    public Brush FillColor { get; set; }
    public ImageSource IconImage { get; set; }
}