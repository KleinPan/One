namespace One.Toolbox.Views.Dialogs
{
    /// <summary> InteractiveDialog.xaml 的交互逻辑 </summary>
    public partial class InteractiveDialogDynamic
    {
        public InteractiveDialogDynamic()
        {
            InitializeComponent();

            //this.LoadViewFromUri("/Aquarius.Common;component/Views/CommonDialog/InteractiveDialogDynamic.xaml");
            Loaded += TestWindow_Loaded;
        }

        private void TestWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}