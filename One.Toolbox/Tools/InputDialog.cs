using One.Toolbox.Views;

using System.Windows.Forms;

namespace One.Toolbox.Tools
{
    internal class InputDialog
    {
        public static Tuple<bool, string> OpenDialog(string prompt, string defaultInput = "", string title = null)
        {
            InputDialogWindow dialog = new InputDialogWindow(prompt, defaultInput, title);
            bool ret = dialog.ShowDialog() ?? false;
            return Tuple.Create(ret, dialog.Value);
        }
    }

    internal class MessageBox
    {
        public static void Show(string s)
        {
            try
            {
                InputDialog.OpenDialog(s, null, null);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(
                    s,
                    "Notice",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    System.Windows.Forms.MessageBoxOptions.ServiceNotification);
            }
        }
    }
}