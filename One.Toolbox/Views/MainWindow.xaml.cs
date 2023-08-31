using Microsoft.Extensions.DependencyInjection;

using One.Toolbox.ViewModels;

using System.Windows.Forms;

namespace One.Toolbox.Views
{
    /// <summary> MainWindow.xaml 的交互逻辑 </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<MainWindowViewModel>();
            ResizeAndRelocate();
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

        /*
        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService,
        IServiceProvider serviceProvider, ISnackbarService snackbarService, IContentDialogService contentDialogService)
        {
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

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
            if (sender is not NavigationView navigationView)
                return;

            NavigationView.HeaderVisibility = navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage)
                ? Visibility.Visible
                : Visibility.Collapsed;

            var targetVM = navigationView.SelectedItem?.TargetPageType.Name;
            if (targetVM != "DashboardPage")
            {
                Analytics.TrackEvent($"{targetVM} clicked.");
            }
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
        */
    }
}