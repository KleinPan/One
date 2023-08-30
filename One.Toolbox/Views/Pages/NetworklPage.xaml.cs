namespace One.Toolbox.Views.Pages
{
    public partial class NetworklPage 
    {
        public ViewModels.NetworkViewModel ViewModel { get; }
        public NetworklPage(ViewModels.NetworkViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;

            InitializeComponent();
        }

     
    }
}