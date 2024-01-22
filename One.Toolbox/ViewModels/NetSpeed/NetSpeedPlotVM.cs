using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;

using One.Toolbox.ViewModels.Base;

using SkiaSharp;

using System.Collections.ObjectModel;

namespace One.Toolbox.ViewModels.NetSpeed;

public partial class NetSpeedPlotVM : BaseVM
{
    public DrawMarginFrame DrawMarginFrame => new()
    {
        Fill = new SolidColorPaint(new SKColor(220, 220, 220)),
        Stroke = new SolidColorPaint(new SKColor(180, 180, 180), 1)
    };

    private readonly List<DateTimePoint> downSpeed = [];
    private readonly List<DateTimePoint> upSpeed = [];

    private readonly DateTimeAxis customXAxis;
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }
    public ObservableCollection<ISeries> SpeedSeries { get; set; }
    public ObservableCollection<RectangularSection> Sections { get; set; } = [];

    public object Sync { get; } = new object();
    private double maxReceiveSpeed { get; set; }
    private double maxSendSpeed { get; set; }

    public NetSpeedPlotVM()
    {
        SpeedSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Name = "Receive",
                Values = downSpeed,
                Fill = new SolidColorPaint( SKColors.CornflowerBlue .WithAlpha(50)),
                Stroke =  new SolidColorPaint( SKColors.CornflowerBlue , 1),
                GeometryFill = null,//坐标点形状
                GeometryStroke =null,
            },
            new LineSeries<DateTimePoint>
            {
                Name = "Send",
                Values = upSpeed,
                Fill =  new SolidColorPaint( SKColors.PaleVioletRed .WithAlpha(50)),
                Stroke=new SolidColorPaint( SKColors.PaleVioletRed , 1),
                GeometryFill = null,
                GeometryStroke =null ,
            }
        };

        customXAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),

            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(20))
        };

        XAxes = [customXAxis];
        YAxes = new Axis[] { new Axis { SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(20)), MinLimit = 0 } };
    }

    private static double[] GetSeparators()
    {
        var now = DateTime.Now;

        List<double> doubles = new List<double>();
        for (var i = 12; i > 0; i--)
        {
            doubles.Add(now.AddSeconds(-i * 5).Ticks);
        }

        doubles.Add(now.Ticks); // Add the current time

        return doubles.ToArray();
    }

    private static string Formatter(DateTime date)
    {
        var secsAgo = (DateTime.Now - date).TotalSeconds;

        return secsAgo < 1
            ? "now"
            : $"{secsAgo:N0}s";
    }

    public void OnSpeedChange(NetSpeedEventArgs netSpeedEventArgs)
    {
        lock (Sync)
        {
            downSpeed.Add(new DateTimePoint(DateTime.Now, netSpeedEventArgs.SpeedReceived));
            if (downSpeed.Count > 61) downSpeed.RemoveAt(0);

            upSpeed.Add(new DateTimePoint(DateTime.Now, netSpeedEventArgs.SpeedSent));
            if (upSpeed.Count > 61) upSpeed.RemoveAt(0);

            customXAxis.CustomSeparators = GetSeparators();
        }

        if (netSpeedEventArgs.SpeedReceived > maxReceiveSpeed)
        {
            maxReceiveSpeed = netSpeedEventArgs.SpeedReceived;

            if (Sections.Count > 0)
            {
                Sections.RemoveAt(0);
            }

            Sections.Insert(0, new RectangularSection()
            {
                //Label = maxReceiveSpeed.ToString(),

                //LabelPaint = new SolidColorPaint(SKColors.CornflowerBlue)
                //{
                //    SKTypeface = SKTypeface.FromFamilyName("Arial")
                //},
                Yi = maxReceiveSpeed,
                Yj = maxReceiveSpeed,
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.CornflowerBlue,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect([6, 6])
                },
            });
        }

        if (netSpeedEventArgs.SpeedSent > maxSendSpeed)
        {
            maxSendSpeed = netSpeedEventArgs.SpeedReceived;

            if (Sections.Count > 1)
            {
                Sections.RemoveAt(1);
            }
            Sections.Add(new RectangularSection()
            {
                //Label = maxSendSpeed.ToString(),
                //LabelPaint = new SolidColorPaint(SKColors.PaleVioletRed)
                //{
                //    SKTypeface = SKTypeface.FromFamilyName("Arial"),
                //},
                Yi = maxSendSpeed,
                Yj = maxSendSpeed,
                Stroke = new SolidColorPaint
                {
                    Color = SKColors.PaleVioletRed,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect([6, 6])
                }
            });
        }
    }
}