namespace One.Toolbox.ViewModels.Serialport
{
    public partial class QuickSendViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        /// <summary> 发送内容 </summary>
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private bool _hex;

        /// <summary> 按钮内容 </summary>
        [ObservableProperty]
        private string _commit;
    }
}