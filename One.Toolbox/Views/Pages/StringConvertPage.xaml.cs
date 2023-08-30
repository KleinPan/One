namespace One.Toolbox.Views.Pages
{
    /// <summary> 编码转换工具页面 </summary>
    public partial class StringConvertPage  
    {
        public ViewModels.StringConvertViewModel ViewModel { get; }

        public StringConvertPage(ViewModels.StringConvertViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;

            InitializeComponent();
        }
    }
}