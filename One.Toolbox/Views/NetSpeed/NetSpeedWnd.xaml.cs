namespace One.Toolbox.Views.NetSpeed
{
    public partial class NetSpeedWnd
    {
        public NetSpeedWnd()
        {
            InitializeComponent();
            Loaded += NewWindow_Loaded; // ע��Loaded�¼��������
        }

        private void NewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!App.Current.MainWindow.ShowActivated)
            //{
            //    Thread.Sleep(1000 * 3);
            //    this.Owner = App.Current.MainWindow;
            //}

            // ������Ļ���½ǵ�λ��
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;

            // ���ô��ڵ�λ��
            Left = screenWidth - windowWidth - 200;
            Top = screenHeight - windowHeight - 200;
        }
    }
}