using HandyControl.Tools.Extension;

using One.Control.Command;
using One.Toolbox.Helpers;

using System.Diagnostics;

using System.Windows.Input;

namespace One.Toolbox.ViewModels.Dialogs
{
    public partial class StringResult : ObservableObject
    {
        [ObservableProperty]
        private DialogResultEnum result;

        [ObservableProperty]
        private string inputText;
    }

    public partial class DialogVMString : DialogVMBase, IDialogResultable<StringResult>
    {
        public DelegateCommand InputCompletedCmd { get; private set; }

        public new StringResult Result { get; set; } = new();

        public DialogVMString()
        {
            InputCompletedCmd = new DelegateCommand(SureEventEx);
        }

        private void SureEventEx(object obj)
        {
            try
            {
                KeyEventArgs key = obj as KeyEventArgs;

                if (key.Key == Key.Enter)
                {
                    Debug.WriteLine("Enter");
                    SureEvent();
                }
            }
            catch (Exception exception)
            {
                MessageShowHelper.ShowErrorMessage(exception.Message);
            }
        }

        public override void SureEvent()
        {
            Result.Result = DialogResultEnum.OK;
            base.SureEvent();
        }

        public override void CloseEvent()
        {
            Result.Result = DialogResultEnum.Cancel;
            base.CloseEvent();
        }
    }
}