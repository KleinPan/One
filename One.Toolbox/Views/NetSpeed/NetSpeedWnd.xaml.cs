namespace One.Toolbox.Views.NetSpeed
{
    public partial class NetSpeedWnd
    {
        public NetSpeedWnd()
        {
            InitializeComponent();
            Loaded += NewWindow_Loaded; // 注册Loaded事件处理程序
        }

        private void NewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.Current.MainWindow.ShowActivated)
            //{
            //    Thread.Sleep(1000 * 3);
            //    this.Owner = App.Current.MainWindow;
            //}

            // 计算屏幕右下角的位置
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;

            // 设置窗口的位置
            Left = screenWidth - windowWidth - 200;
            Top = screenHeight - windowHeight - 200;
        }
    }
}