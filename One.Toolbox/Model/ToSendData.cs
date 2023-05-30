namespace One.Toolbox.Model
{
    public partial class ToSendData : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private bool _hex;

        [ObservableProperty]
        private string _commit;
    }
}