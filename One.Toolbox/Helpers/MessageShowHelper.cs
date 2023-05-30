using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.IconElements;

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
            _snackbarService.Timeout = _snackbarTimeout;
        }

        public static void ShowErrorMessage(string message)
        {
            _snackbarService.Show("Error", message, new SymbolIcon(SymbolRegular.Fluent24), _snackbarAppearance);
        }
    }
}