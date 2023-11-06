using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace One.Control.Controls.CirclePanel;

[TemplatePart(Name = BorderTemplateName, Type = typeof(Border))]
[TemplatePart(Name = ItemsControlAngleName, Type = typeof(RotateTransform))]
[TemplatePart(Name = ItemControlName, Type = typeof(ItemsControl))]
public class DrawPrize : ListBox
{
    private const string BorderTemplateName = "PART_Border";
    private const string ItemsControlAngleName = "PART_ItemsControlAngle";

    private const string ItemControlName = "PART_ItemsControl";

    private Border _border;
    private RotateTransform itemsControlAngle;

    static DrawPrize()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawPrize),
            new FrameworkPropertyMetadata(typeof(DrawPrize)));
    }

    public DrawPrize()
    {
        Debug.WriteLine(ChildCount);
    }

    private int ChildCount;

    private ItemsControl itemsControl;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _border = GetTemplateChild(BorderTemplateName) as Border;
        itemsControlAngle = GetTemplateChild(ItemsControlAngleName) as RotateTransform;
        _border.MouseDown += _border_MouseDown;
        itemsControl = GetTemplateChild(ItemControlName) as ItemsControl;

        ChildCount = itemsControl.Items.Count;

        //这里拿不到
        //var b = MyVisualTreeHelper.FindVisualChild<PrizeItemControl>(itemsControl as DependencyObject);

        int a = 2000;
        for (int i = 0; i < ChildCount; i++)
        {
            ListAngle.Add(a + i * 45);
        }
    }

    private int value;
    private List<int> ListAngle = new List<int>();

    private void _border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        //子控件渲染完了才能拿到
        //var b = MyVisualTreeHelper.FindVisualChild<PrizeItemControl>(itemsControl as DependencyObject);

        _border.IsEnabled = false;
        _border.Cursor = Cursors.None;
        var random = new Random();
        var to = random.Next(0, ChildCount);
        var doubleAnimation = new DoubleAnimationUsingKeyFrames();

        value = ListAngle[to];

        var splineDoubleKey1 = new SplineDoubleKeyFrame
        {
            KeyTime = TimeSpan.FromSeconds(0),
            Value = value % 360
        };
        var splineDoubleKey2 = new SplineDoubleKeyFrame
        {
            KeyTime = TimeSpan.FromMilliseconds(1000),
            Value = 360
        };
        var splineDoubleKey3 = new SplineDoubleKeyFrame
        {
            KeyTime = TimeSpan.FromMilliseconds(2000),
            Value = 1230
        };
        var splineDoubleKey4 = new SplineDoubleKeyFrame
        {
            KeyTime = TimeSpan.FromMilliseconds(4000),
            Value = value,
            KeySpline = new KeySpline(0, 0, 0, 1)
        };
        doubleAnimation.KeyFrames.Add(splineDoubleKey1);
        doubleAnimation.KeyFrames.Add(splineDoubleKey2);
        doubleAnimation.KeyFrames.Add(splineDoubleKey3);
        doubleAnimation.KeyFrames.Add(splineDoubleKey4);
        doubleAnimation.Completed += (s1, e1) =>
        {
            _border.IsEnabled = true;
            _border.Cursor = Cursors.Hand;
        };
        itemsControlAngle.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
    }
}