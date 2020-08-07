using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace One.Control.ExtentionMethods
{
    /// <summary>
    /// 滚动扩展方法
    /// </summary>
   public static class ItemControlExtentionMethods
    {
        #region 水平
        public static double GetHorizontalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalOffsetProperty);
        }

        public static void SetHorizontalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalOffsetProperty, value);
        }

        /// <summary> 动画属性，控制水平方向移动 </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ItemControlExtentionMethods), new UIPropertyMetadata(0.0, OnHorizontalOffsetPropertyChanged));

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ScrollViewer)sender).ScrollToHorizontalOffset((double)args.NewValue);
        }

        #endregion


        #region 垂直

        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }

        // 用一个依赖属性作为verticaloffset存储器. 这使动画, 样式,绑定, 等等及其他
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ItemControlExtentionMethods), new UIPropertyMetadata(0.0, OnVerticalOffsetPropertyChanged));

        private static void OnVerticalOffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((ScrollViewer)sender).ScrollToVerticalOffset((double)args.NewValue);
        }

        #endregion

        public static void ScrollToSelectedItem(this ItemsControl listbox, object selectedItem)
        {
            ScrollViewer scrollHost = VisualTreeHelper.GetChild(listbox, 0) as ScrollViewer;
            for (DependencyObject obj2 = listbox; obj2 != null; obj2 = VisualTreeHelper.GetChild(obj2, 0))
            {
                ScrollViewer viewer = obj2 as ScrollViewer;
                if (viewer != null)
                {
                    scrollHost = viewer;
                    break;
                }
            }
            if (scrollHost != null && scrollHost.IsLoaded)
            {
                double fromValue = scrollHost.HorizontalOffset;
                double toValue = 0.0;
                if (selectedItem != null)
                {
                    FrameworkElement visual = listbox.ItemContainerGenerator.ContainerFromItem(selectedItem) as FrameworkElement;
                    if (visual != null)
                    {
                        Vector vector = VisualTreeHelper.GetOffset(visual);
                        toValue = (visual.ActualWidth - scrollHost.ViewportWidth) / 2 + vector.X;
                    }
                }
                DoubleAnimation animation = new DoubleAnimation(fromValue, toValue, new TimeSpan(0, 0, 0, 0, 500), FillBehavior.HoldEnd);
                scrollHost.BeginAnimation(ItemControlExtentionMethods.HorizontalOffsetProperty, animation);
            }
        }

        public static void ScrollSkip(this ItemsControl listbox, int count)
        {
            ScrollViewer scrollHost = VisualTreeHelper.GetChild(listbox, 0) as ScrollViewer;
            for (DependencyObject obj2 = listbox; obj2 != null; obj2 = VisualTreeHelper.GetChild(obj2, 0))
            {
                ScrollViewer viewer = obj2 as ScrollViewer;
                if (viewer != null)
                {
                    scrollHost = viewer;
                    break;
                }
            }
            if (scrollHost != null && scrollHost.IsLoaded)
            {
                double fromHorizontalOffset = scrollHost.HorizontalOffset;
                double toHorizontalOffset = 0.0;
                double fromVerticalOffset = scrollHost.VerticalOffset;
                double toVerticalOffset = 0.0;
                toHorizontalOffset = (scrollHost.ExtentWidth / listbox.Items.Count) * count + fromHorizontalOffset;
                toVerticalOffset = (scrollHost.ExtentHeight / listbox.Items.Count) * count + fromVerticalOffset;

                if (ScrollViewer.GetHorizontalScrollBarVisibility(listbox) != ScrollBarVisibility.Disabled)
                {
                    DoubleAnimation animation = new DoubleAnimation(fromHorizontalOffset, toHorizontalOffset, new TimeSpan(0, 0, 0, 0, 800), FillBehavior.HoldEnd);
                    scrollHost.BeginAnimation(ItemControlExtentionMethods.HorizontalOffsetProperty, animation);
                }
                if (ScrollViewer.GetVerticalScrollBarVisibility(listbox) != ScrollBarVisibility.Disabled)
                {
                    DoubleAnimation animation = new DoubleAnimation(fromVerticalOffset, toVerticalOffset, new TimeSpan(0, 0, 0, 0, 800), FillBehavior.HoldEnd);
                    scrollHost.BeginAnimation(ItemControlExtentionMethods.VerticalOffsetProperty, animation);
                }
            }
        }

        public static void ScrollSkip_Double(this ItemsControl listbox, Double count)
        {
            ScrollViewer scrollHost = VisualTreeHelper.GetChild(listbox, 0) as ScrollViewer;
            for (DependencyObject obj2 = listbox; obj2 != null; obj2 = VisualTreeHelper.GetChild(obj2, 0))
            {
                ScrollViewer viewer = obj2 as ScrollViewer;
                if (viewer != null)
                {
                    scrollHost = viewer;
                    break;
                }
            }
            if (scrollHost != null && scrollHost.IsLoaded)
            {
                double fromHorizontalOffset = scrollHost.HorizontalOffset;
                double toHorizontalOffset = 0.0;
                double fromVerticalOffset = scrollHost.VerticalOffset;
                double toVerticalOffset = 0.0;
                toHorizontalOffset = (scrollHost.ExtentWidth / listbox.Items.Count) * count + fromHorizontalOffset;
                toVerticalOffset = (scrollHost.ExtentHeight / listbox.Items.Count) * count + fromVerticalOffset;

                if (ScrollViewer.GetHorizontalScrollBarVisibility(listbox) != ScrollBarVisibility.Disabled)
                {
                    DoubleAnimation animation = new DoubleAnimation(fromHorizontalOffset, toHorizontalOffset, new TimeSpan(0, 0, 0, 0, 800), FillBehavior.HoldEnd);
                    scrollHost.BeginAnimation(ItemControlExtentionMethods.HorizontalOffsetProperty, animation);
                }
                if (ScrollViewer.GetVerticalScrollBarVisibility(listbox) != ScrollBarVisibility.Disabled)
                {
                    DoubleAnimation animation = new DoubleAnimation(fromVerticalOffset, toVerticalOffset, new TimeSpan(0, 0, 0, 0, 800), FillBehavior.HoldEnd);
                    scrollHost.BeginAnimation(ItemControlExtentionMethods.VerticalOffsetProperty, animation);
                }
            }
        }
    }
}
