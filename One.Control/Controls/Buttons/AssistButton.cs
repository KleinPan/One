using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace One.Control.Controls.Buttons
{
    

    [TemplatePart(Name = "PART_PGrid", Type = typeof(Grid))]
    public class AssistButton : Button
    {
        static AssistButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssistButton), new FrameworkPropertyMetadata(typeof(AssistButton)));
        }

        public event EventHandler ClickEvent;

        private bool _move = false;

        //double MinMagneticSuctionDist = 50;//吸附边距
        //double MinEdgeDist = 5;//固定边距
        private Point _lastPos;//移动前位置

        private Point _newPos;
        private Point _oldPos;

        public AssistButton()
        {
            this.Loaded += FloatButton2_Loaded;
            this.Click += btn_Click;
        }

        private Grid pgrid;

        private Grid PGrid
        {
            get { return pgrid; }
            set
            {
                if (pgrid != null)
                {
                    pgrid.MouseLeftButtonDown -= new MouseButtonEventHandler(FloatButton2_MouseLeftButtonDown);
                }
                pgrid = value;

                if (pgrid != null)
                {
                    pgrid.MouseLeftButtonDown += new MouseButtonEventHandler(FloatButton2_MouseLeftButtonDown);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PGrid = GetTemplateChild("PART_PGrid") as Grid;
            //textBlock = GetTemplateChild("txb") as TextBlock;
            //textBlockm = GetTemplateChild("txbm") as TextBlock;
            //textBlocklast = GetTemplateChild("txblast") as TextBlock;
        }

        private void FloatButton2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Parent != null && this.Parent is FrameworkElement)
            {
                FrameworkElement parent = this.Parent as FrameworkElement;
                _move = true;
                _lastPos = e.GetPosition(parent);
                _oldPos = _lastPos;
            }
        }

        /// <summary> 最小边距 </summary>
        public double MinEdgeDist
        {
            get { return (double)GetValue(MinEdgeDistProperty); }
            set { SetValue(MinEdgeDistProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinEdgeDist. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinEdgeDistProperty =
            DependencyProperty.Register("MinEdgeDist", typeof(double), typeof(AssistButton), new PropertyMetadata(50.0));

        /// <summary> 最小磁吸距离 </summary>
        public double MinMagneticSuctionDist
        {
            get { return (double)GetValue(MinMagneticSuctionDistProperty); }
            set { SetValue(MinMagneticSuctionDistProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinMagneticSuctionDist. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinMagneticSuctionDistProperty =
            DependencyProperty.Register("MinMagneticSuctionDist", typeof(double), typeof(AssistButton), new PropertyMetadata(5.0));

        // public static readonlyRoutedEvent
        private void FloatButton2_Loaded(object sender, RoutedEventArgs e)
        {
            if (MinEdgeDist > MinMagneticSuctionDist)
            {
                MinMagneticSuctionDist = MinEdgeDist * 2;
            }
            if (this.Parent != null && this.Parent is FrameworkElement)
            {
                FrameworkElement parent = this.Parent as FrameworkElement;
                double left1 = parent.ActualWidth - PGrid.ActualWidth - this.MinEdgeDist;
                double top1 = parent.ActualHeight - PGrid.ActualHeight - this.MinEdgeDist;
                this.Margin = new Thickness(left1, top1, 0, 0);

                parent.PreviewMouseMove += (s, ee) =>
                {
                    if (_move)
                    {
                        Point pos = ee.GetPosition(parent);
                        double left = this.Margin.Left + pos.X - this._lastPos.X;
                        double top = this.Margin.Top + pos.Y - this._lastPos.Y;
                        this.Margin = new Thickness(left, top, 0, 0);

                        _lastPos = ee.GetPosition(parent);
                    }
                };

                parent.PreviewMouseUp += (s, ee) =>
                {
                    if (_move)
                    {
                        _move = false;

                        Point pos = ee.GetPosition(parent);
                        _newPos = pos;
                        double left = this.Margin.Left + pos.X - this._lastPos.X;
                        double top = this.Margin.Top + pos.Y - this._lastPos.Y;
                        double right = parent.ActualWidth - left - PGrid.ActualWidth;
                        double bottom = parent.ActualHeight - top - PGrid.ActualHeight;

                        //textBlocklast.Text = "l.X:" + _lastPos.X + "l.Y: " + _lastPos.Y;
                        //textBlockm.Text = "M.l:" + Margin.Left + "M.t: " +Margin.Top;
                        //textBlock.Text = "left:" + left + "top: " + top;

                        if (left < MinMagneticSuctionDist && top < MinMagneticSuctionDist) //左上
                        {
                            left = this.MinEdgeDist;
                            top = this.MinEdgeDist;
                        }
                        else if (left < MinMagneticSuctionDist && bottom < MinMagneticSuctionDist) //左下
                        {
                            left = this.MinEdgeDist;
                            top = parent.ActualHeight - PGrid.ActualHeight - this.MinEdgeDist;
                        }
                        else if (right < MinMagneticSuctionDist && top < MinMagneticSuctionDist) //右上
                        {
                            left = parent.ActualWidth - PGrid.ActualWidth - this.MinEdgeDist;
                            top = this.MinEdgeDist;
                        }
                        else if (right < MinMagneticSuctionDist && bottom < MinMagneticSuctionDist) //右下
                        {
                            left = parent.ActualWidth - PGrid.ActualWidth - this.MinEdgeDist;
                            top = parent.ActualHeight - PGrid.ActualHeight - this.MinEdgeDist;
                        }
                        else if (left < MinMagneticSuctionDist && top > MinMagneticSuctionDist && bottom > MinMagneticSuctionDist) //左
                        {
                            left = this.MinEdgeDist;
                            top = this.Margin.Top;
                        }
                        else if (right < MinMagneticSuctionDist && top > MinMagneticSuctionDist && bottom > MinMagneticSuctionDist) //右
                        {
                            left = parent.ActualWidth - PGrid.ActualWidth - this.MinEdgeDist;
                            top = this.Margin.Top;
                        }
                        else if (top < MinMagneticSuctionDist && left > MinMagneticSuctionDist && right > MinMagneticSuctionDist) //上
                        {
                            left = this.Margin.Left;
                            top = this.MinEdgeDist;
                        }
                        else if (bottom < MinMagneticSuctionDist && left > MinMagneticSuctionDist && right > MinMagneticSuctionDist) //下
                        {
                            left = this.Margin.Left;
                            top = parent.ActualHeight - PGrid.ActualHeight - this.MinEdgeDist;
                        }

                        ThicknessAnimation marginAnimation = new ThicknessAnimation();
                        marginAnimation.From = this.Margin;
                        marginAnimation.To = new Thickness(left, top, 0, 0);
                        marginAnimation.Duration = TimeSpan.FromMilliseconds(200);

                        Storyboard story = new Storyboard();
                        story.FillBehavior = FillBehavior.Stop;
                        story.Children.Add(marginAnimation);
                        //Storyboard.SetTargetName(marginAnimation, "btn");
                        Storyboard.SetTarget(marginAnimation, this);
                        Storyboard.SetTargetProperty(marginAnimation, new PropertyPath("(0)", Border.MarginProperty));

                        story.Begin(this);

                        this.Margin = new Thickness(left, top, 0, 0);
                    }
                };
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (_newPos.Equals(_oldPos))
            {
                if (ClickEvent != null)
                {
                    ClickEvent(sender, e);
                }
            }
        }
    }
}
