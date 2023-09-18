using HandyControl.Controls;
using HandyControl.Data;

using System.Diagnostics;

namespace One.Toolbox.Helpers
{
    internal class MessageShowHelper
    {
        static MessageShowHelper()
        {
        }

        public static void ShowErrorMessage(string message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
            });

            HandyControl.Controls.Growl.Error(new GrowlInfo
            {
                Message = $"{message}",
            });
        }

        public static void ShowWarnMessage(string message)
        {
            HandyControl.Controls.Growl.Warning(new GrowlInfo
            {
                Message = $"{message}",
            });
        }

        public static void ShowInfoMessage(string message)
        {
            HandyControl.Controls.Growl.Info(new GrowlInfo
            {
                Message = $"{message}",
            });
        }
    }
}