using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace One.Controls.Controls.MessageBox
{
    /// <summary> TextMessageBox.xaml 的交互逻辑 </summary>
    public partial class PopUpMessageBox : Window
    {
        /// <summary> 禁止在外部实例化 </summary>
        private PopUpMessageBox()
        {
            InitializeComponent();
            this.Loaded += NotificationWindow_Loaded;
        }

        /// <summary> 标题 </summary>
        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string Message
        {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        public double TopFrom
        {
            get; set;
        }

        public static void Show(string title, string msg)
        {
            PopUpMessageBox popUpMessageBox = new PopUpMessageBox();

            popUpMessageBox.Title = title;
            popUpMessageBox.Message = msg;

            popUpMessageBox.TopFrom = GetTopFrom();

            popUpMessageBox.Show();

            _dialogs.Add(popUpMessageBox);
        }

        private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PopUpMessageBox self = sender as PopUpMessageBox;
            if (self != null)
            {
                self.UpdateLayout();
                SystemSounds.Asterisk.Play();//播放提示声

                double right = SystemParameters.WorkArea.Right;//工作区最右边的值
                self.Top = self.TopFrom - self.ActualHeight;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
                animation.From = right;
                animation.To = right - self.ActualWidth;//设定通知从右往左弹出
                self.BeginAnimation(Window.LeftProperty, animation);//设定动画应用于窗体的Left属性

                Task.Factory.StartNew(delegate
                {
                    int seconds = 5;//通知持续5s后消失
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    //Invoke到主进程中去执行
                    this.Dispatcher.Invoke(delegate
                    {
                        animation = new DoubleAnimation();
                        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                        animation.Completed += (s, a) => { self.Close(); };//动画执行完毕，关闭当前窗体
                        animation.From = right - self.ActualWidth;
                        animation.To = right;//通知从左往右收回
                        self.BeginAnimation(Window.LeftProperty, animation);
                        _dialogs.Remove(this);
                    });
                });
            }
        }

        private static List<PopUpMessageBox> _dialogs = new List<PopUpMessageBox>();

        private static double GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = System.Windows.SystemParameters.WorkArea.Bottom;
            bool isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 130;//此处100是NotifyWindow的高
                isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
            {
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom;
            }

            return topFrom;
        }

        private void Boder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double right = SystemParameters.WorkArea.Right;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            animation.Completed += (s, a) => { this.Close(); };
            animation.From = right - this.ActualWidth;
            animation.To = right;
            this.BeginAnimation(Window.LeftProperty, animation);

            _dialogs.Remove(this);
        }
    }
}