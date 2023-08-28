namespace One.Toolbox.Views.Pages
{
    public partial class NetworklPage : INavigableView<ViewModels.NetworkViewModel>
    {
        public NetworklPage(ViewModels.NetworkViewModel viewModel)
        {
            DataContext = ViewModel = viewModel;

            InitializeComponent();
        }

        public ViewModels.NetworkViewModel ViewModel { get; }
    }
}