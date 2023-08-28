using Wpf.Ui;

namespace One.Toolbox.Helpers
{
    internal class MessageShowHelper
    {
        private static readonly ISnackbarService _snackbarService;
        private static int _snackbarTimeout = 2000;

        private static ControlAppearance _snackbarAppearance = ControlAppearance.Secondary;

        static MessageShowHelper()
        {
            _snackbarService = App.GetService<ISnackbarService>();
            //_snackbarService.Timeout = _snackbarTimeout;
        }

        public static void ShowErrorMessage(string message)
        {
            // _snackbarService.Show("Error", message, new SymbolIcon(SymbolRegular.Fluent24), _snackbarAppearance);

            App.Current.Dispatcher.Invoke(() =>
            {
                _snackbarService.Show("Error", message, _snackbarAppearance, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(_snackbarTimeout));
            });
        }

        public static void ShowInfoMessage(string message)
        {
            // _snackbarService.Show("Error", message, new SymbolIcon(SymbolRegular.Fluent24), _snackbarAppearance);

            App.Current.Dispatcher.Invoke(() =>
            {
                _snackbarService.Show("Info", message, _snackbarAppearance, new SymbolIcon(SymbolRegular.Fluent24), TimeSpan.FromSeconds(_snackbarTimeout));
            });
        }
    }
}