using HandyControl.Tools.Extension;

namespace One.Toolbox.ViewModels.Dialogs
{
    public class DialogVMString : DialogVMBase, IDialogResultable<string>
    {
        public new string Result { get; set; }

        public DialogVMString()
        {
        }
    }
}