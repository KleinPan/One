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

        public static void ShowErrorMessage(string message, bool global = false)
        {
            if (global)
            {
                HandyControl.Controls.Growl.ErrorGlobal(new GrowlInfo
                {
                    Message = $"{message}",
                });
            }
            else
            {
                HandyControl.Controls.Growl.Error(new GrowlInfo
                {
                    Message = $"{message}",
                });
            }
            App.Current.Dispatcher.Invoke(() =>
            {
            });
        }

        public static void ShowWarnMessage(string message)
        {
            HandyControl.Controls.Growl.Warning(new GrowlInfo
            {
                Message = $"{message}",
            });
        }

        public static void ShowInfoMessage(string message, bool global = false)
        {
            if (global)
            {
                HandyControl.Controls.Growl.InfoGlobal(new GrowlInfo
                {
                    Message = $"{message}",
                });
            }
            else
            {
                HandyControl.Controls.Growl.Info(new GrowlInfo
                {
                    Message = $"{message}",
                });
            }
        }
    }
}