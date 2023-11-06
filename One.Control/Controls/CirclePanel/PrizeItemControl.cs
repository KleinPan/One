using One.Control.Helpers;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace One.Control.Controls.CirclePanel;

[TemplatePart(Name = RotateTransformTemplateName, Type = typeof(RotateTransform))]
[TemplatePart(Name = PART_PathName, Type = typeof(Path))]
public class PrizeItemControl : System.Windows.Controls.Control
{
    private const string RotateTransformTemplateName = "PART_RotateTransform";
    private const string PART_PathName = "PART_Path";
    private const string PART_TxtTranslateTransform = "PART_TxtTranslateTransform";
    private const string PART_TxtRotateTransform = "PART_TxtRotateTransform";

    private const string PART_Text = "PART_Text";

    private RotateTransform _angleRotateTransform;
    private Path pathSector;
    private TranslateTransform txtTranslateTransform;
    private TextBlock text;
    private RotateTransform txtRotateTransform;

    #region DependencyProperty

    private static readonly Type _typeofSelf = typeof(PrizeItemControl);

    public int Angle
    {
        get => (int)GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    public static readonly DependencyProperty AngleProperty =
        DependencyProperty.Register("Angle", typeof(int), typeof(PrizeItemControl), new UIPropertyMetadata(OnAngleChanged));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(PrizeItemControl), new PropertyMetadata(string.Empty));

    public Brush BackgroundColor
    {
        get => (Brush)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public static readonly DependencyProperty BackgroundColorProperty =
        DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(PrizeItemControl), new PropertyMetadata(null));

    public int StartAngle
    {
        get { return (int)GetValue(StartAngleProperty); }
        set { SetValue(StartAngleProperty, value); }
    }

    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register("StartAngle", typeof(int), typeof(PrizeItemControl), new UIPropertyMetadata(OnStartAngleChanged));

    #endregion DependencyProperty

    static PrizeItemControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
    }

    private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PrizeItemControl)d;
        control.UpdateStartAngle();
    }

    private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PrizeItemControl)d;
        control.UpdateAngle();
    }

    private int radius = 150;

    private int centerX = 200;
    private int centerY = 200;

    /// <summary> 画扇形 </summary>
    /// <param name="du"> </param>
    public void DrawSector()
    {
        // Create a figure.
        PathFigure myPathFigure = new PathFigure();
        myPathFigure.IsClosed = true;
        myPathFigure.StartPoint = new Point(centerX, centerY);//起点  长度120

        Point orinianlStartPoint = new Point(centerX - radius, centerY);

        #region ArcStart

        Matrix matrix = new Matrix();
        matrix.RotateAt(StartAngle, centerX, centerY);

        var arcStart = matrix.Transform(orinianlStartPoint);

        myPathFigure.Segments.Add(new LineSegment(arcStart, isStroked: true));

        #endregion ArcStart

        #region ArcCenter

        matrix = new Matrix();
        matrix.RotateAt(StartAngle + Angle / 2, centerX, centerY);

        var arcCenter = matrix.Transform(orinianlStartPoint);

        #endregion ArcCenter

        #region ArcEnd

        matrix = new Matrix();
        matrix.RotateAt(StartAngle + Angle, centerX, centerY);

        var arcEnd = matrix.Transform(orinianlStartPoint);

        #endregion ArcEnd

        myPathFigure.Segments.Add(new ArcSegment(
            arcEnd,
            new Size(radius, radius),
            45,
            false, /* IsLargeArc */
            SweepDirection.Clockwise,
            true /* IsStroked */ ));

        PathGeometry myPathGeometry = new PathGeometry();
        myPathGeometry.Figures.Add(myPathFigure);

        //myPathFigure.Segments.Add(new LineSegment());

        pathSector.Stroke = Brushes.Black;
        pathSector.StrokeThickness = 1;
        pathSector.Data = myPathGeometry;

        if (txtTranslateTransform != null)
        {
            txtRotateTransform.Angle = (StartAngle + Angle / 2) - 90;

            var txtX = ((arcStart.X + arcEnd.X) / 2 + arcCenter.X) / 2;
            var txtY = ((arcStart.Y + arcEnd.Y) / 2 + arcCenter.Y) / 2;

            //var w = text.ActualWidth / 2;
            //var h = text.ActualHeight / 2;
            txtTranslateTransform.X = (txtX);
            txtTranslateTransform.Y = (txtY);
            //txtTranslateTransform.X = arcCenter.X;
            //txtTranslateTransform.Y = arcCenter.Y;
        }
    }

    private void UpdateStartAngle()
    {
        if (_angleRotateTransform == null) return;
        //_angleRotateTransform.Angle = Angle;
    }

    private void UpdateAngle()
    {
        if (_angleRotateTransform == null) return;
        //_angleRotateTransform.Angle = Angle;
    }

    public override void OnApplyTemplate()
    {
        //设置步骤 2 中在 OnApplyTemplate 方法中定义的 FrameworkElement 属性。 这是 ControlTemplate 中的 FrameworkElement 可供控件使用的最早时间。
        //使用 FrameworkElement 的 x:Name 从 ControlTemplate 获取它。
        base.OnApplyTemplate();

        _angleRotateTransform = GetTemplateChild(RotateTransformTemplateName) as RotateTransform;
        pathSector = GetTemplateChild(PART_PathName) as Path;

        text = GetTemplateChild(PART_Text) as TextBlock;

        txtRotateTransform = GetTemplateChild(PART_TxtRotateTransform) as RotateTransform;
        txtTranslateTransform = GetTemplateChild(PART_TxtTranslateTransform) as TranslateTransform;
        var a = MyVisualTreeHelper.FindVisualParent2<DrawPrize>(this);
        if (a != null)
        {
            var index = a.Items.IndexOf(this.DataContext);

            // for (int i = 0; i < a.Items.Count; i++) { var b = a.Items[i] as LotteryDrawModel; beforeAdd += } //DrawSector(360 / a.ChildCount);
        }
        UpdateStartAngle();
        UpdateAngle();
        DrawSector();
    }
}