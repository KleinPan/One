using One.Toolbox.Interfaces;
using One.Toolbox.ViewModels;
using One.Toolbox.Views.Pages;

using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Services;

namespace One.Toolbox.Views
{
    /// <summary> MainWindow.xaml 的交互逻辑 </summary>
    public partial class MainWindow : IWindow
    {
        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService,
        IServiceProvider serviceProvider, ISnackbarService snackbarService, Wpf.Ui.Contracts.IContentDialogService contentDialogService)
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            DataContext = ViewModel = viewModel;

            InitializeComponent();

            //snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            //isnackbarService.SetSnackbarControl(SnackbarPresenter);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            navigationService.SetNavigationControl(NavigationView);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);
            NavigationView.Loaded += (_, _) => NavigationView.Navigate(typeof(DashboardPage));
        }

        public MainWindowViewModel ViewModel { get; }

        private bool _isUserClosedPane;
        private bool _isPaneOpenedOrClosedFromCode;

        private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.Navigation.NavigationView navigationView)
                return;

            NavigationView.HeaderVisibility = navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isUserClosedPane)
                return;

            _isPaneOpenedOrClosedFromCode = true;
            NavigationView.IsPaneOpen = !(e.NewSize.Width <= 1200);
            _isPaneOpenedOrClosedFromCode = false;
        }

        private void NavigationView_OnPaneOpened(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
                return;

            _isUserClosedPane = false;
        }

        private void NavigationView_OnPaneClosed(NavigationView sender, RoutedEventArgs args)
        {
            if (_isPaneOpenedOrClosedFromCode)
                return;

            _isUserClosedPane = true;
        }
    }
}