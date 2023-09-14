using HandyControl.Controls;
using HandyControl.Tools.Extension;

using One.Toolbox.ViewModels.Dialogs;
using One.Toolbox.Views.Dialogs;

namespace One.Toolbox.Helpers
{
    public class DialogHelper
    {
        public static DialogHelper Instance = new Lazy<DialogHelper>(() => new DialogHelper()).Value;

        /// <summary> 使用方式在ui层中，代async的方法里加上 await </summary>
        /// <param name="header"> </param>
        /// <returns> </returns>
        public async Task<string> ShowInteractiveDialog(string info, string title = "Info", string res = "")
        {
            var myDialog = Dialog.Show<InteractiveDialog>();

            myDialog.DataContext = new DialogVMString()
            {
                Title = title,
                Info = info,
                Result = res,
            };
            return await myDialog.GetResultAsync<string>();
        }

        public async Task<Dictionary<string, string>> ShowInputDialog(string title, List<InputInfoVM> inputInfoMs)
        {
            //bool? diares = false;

            var myDialog = Dialog.Show<InteractiveDialogDynamic>();

            var vm = new DialogVMDynamic()
            {
                Title = title,
            };

            myDialog.DataContext = vm;
            vm.IniDialog(inputInfoMs);

            return await myDialog.GetResultAsync<Dictionary<string, string>>();

            /*
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                InteractiveDialogDynamic interactiveWndDynamic = new InteractiveDialogDynamic();
                interactiveWndDynamic.Owner = Application.Current.MainWindow;

                interactiveWndDynamic.Title = title;

                interactiveWndDynamic.InitWnd2(inputInfoMs);

                diares = interactiveWndDynamic.ShowDialog();

                if ((bool)diares)
                {
                    keyValuePairs = interactiveWndDynamic.OutputDictionary;
                }
                else
                {
                    throw new Exception("GetInputInfo fail!");
                }
            }));

            return keyValuePairs;
            */
        }
    }
}