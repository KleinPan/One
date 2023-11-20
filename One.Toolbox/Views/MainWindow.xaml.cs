using HandyControl.Tools;
using HandyControl.Tools.Extension;

using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;
using One.Toolbox.ViewModels.MainWindow;
using One.Toolbox.Views.Settings;

using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace One.Toolbox.Views
{
    /// <summary> MainWindow.xaml 的交互逻辑 </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.DataContext = App.Current.Services.GetService<MainWindowVM>();
            //ResizeAndRelocate();

            NonClientAreaContent = new NonClientAreaContent();

            InitializeComponent();
        }

        private void ResizeAndRelocate()
        {
            Screen screen = Screen.PrimaryScreen;
            // 获取屏幕的宽度和高度
            int screenWidth = screen.Bounds.Width;
            int screenHeight = screen.Bounds.Height;
            Console.WriteLine("屏幕宽度：" + screenWidth);
            Console.WriteLine("屏幕高度：" + screenHeight);

            this.Width = screenWidth / 4;
            this.Height = screenHeight / 3;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private GridLength _columnDefinitionWidth;

        private void OnLeftMainContentShiftOut(object sender, RoutedEventArgs e)
        {
            ButtonLeft.Collapse();
            GridSplitter.IsEnabled = false;

            double targetValue = -ColumnDefinitionLeft.MaxWidth;
            _columnDefinitionWidth = ColumnDefinitionLeft.Width;

            DoubleAnimation animation = AnimationHelper.CreateAnimation(targetValue, milliseconds: 100);
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += OnAnimationCompleted;
            LeftMainContent.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            void OnAnimationCompleted(object _, EventArgs args)
            {
                animation.Completed -= OnAnimationCompleted;
                LeftMainContent.RenderTransform.SetCurrentValue(TranslateTransform.XProperty, targetValue);

                Grid.SetColumn(MainContent, 0);
                Grid.SetColumnSpan(MainContent, 2);

                ColumnDefinitionLeft.MinWidth = 0;
                ColumnDefinitionLeft.Width = new GridLength();
                ButtonRight.Show();
            }
        }

        private void OnLeftMainContentShiftIn(object sender, RoutedEventArgs e)
        {
            ButtonRight.Collapse();
            GridSplitter.IsEnabled = true;

            double targetValue = ColumnDefinitionLeft.Width.Value;

            DoubleAnimation animation = AnimationHelper.CreateAnimation(targetValue, milliseconds: 100);
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += OnAnimationCompleted;
            LeftMainContent.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            void OnAnimationCompleted(object _, EventArgs args)
            {
                animation.Completed -= OnAnimationCompleted;
                LeftMainContent.RenderTransform.SetCurrentValue(TranslateTransform.XProperty, targetValue);

                Grid.SetColumn(MainContent, 1);
                Grid.SetColumnSpan(MainContent, 1);

                ColumnDefinitionLeft.MinWidth = 45;
                ColumnDefinitionLeft.Width = _columnDefinitionWidth;
                ButtonLeft.Show();
            }
        }
    }
}