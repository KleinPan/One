namespace One.Toolbox.ViewModels.Serialport
{
    public partial class QuickSendViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        /// <summary> �������� </summary>
        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private bool _hex;

        /// <summary> ��ť���� </summary>
        [ObservableProperty]
        private string _commit;
    }
}