using HandyControl.Tools.Extension;

using One.Control.Command;
using One.Toolbox.Helpers;
using One.Toolbox.ViewModels.Base;

using System.Diagnostics;
using System.Windows.Input;

namespace One.Toolbox.ViewModels.Dialogs
{
    public partial class DialogVMBase : BaseViewModel, IDialogResultable<DialogResultEnum>
    {
        public DelegateCommand CloseCmd { get; private set; }
        public DelegateCommand SureCmd { get; private set; }

        public DelegateCommand InputCompletedCmd { get; private set; }

        public DialogVMBase()
        {
            CloseCmd = new DelegateCommand(CloseEvent);

            SureCmd = new DelegateCommand(SureEvent);

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

        public virtual void SureEvent()
        {
            Result = DialogResultEnum.OK;
            CloseAction?.Invoke();
        }

        public void CloseEvent()
        {
            Result = DialogResultEnum.Cancel;
            CloseAction?.Invoke();
        }

        public Action CloseAction { get; set; }

        [ObservableProperty]
        private string info;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private DialogResultEnum result;
    }
}