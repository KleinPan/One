namespace One.Toolbox.Views.Dialogs
{
    /// <summary> InteractiveDialog.xaml 的交互逻辑 </summary>
    public partial class InteractiveDialog
    {
        public InteractiveDialog()
        {
            InitializeComponent();

            //this.LoadViewFromUri("/Aquarius.Common;component/Views/CommonDialog/InteractiveDialog.xaml");
            Loaded += TestWindow_Loaded;
        }

        private void TestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            inputtxb.Focus();
        }
    }
}