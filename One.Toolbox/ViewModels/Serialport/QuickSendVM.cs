using Microsoft.Extensions.DependencyInjection;

using One.Base.Helpers.DataProcessHelpers;
using One.Toolbox.Helpers;

namespace One.Toolbox.ViewModels.Serialport;

public partial class QuickSendVM : ObservableObject
{
    [ObservableProperty]
    private int _id;

    /// <summary>��������</summary>
    [ObservableProperty]
    private string _text;

    [ObservableProperty]
    private bool _hex;

    /// <summary>��ť����</summary>
    [ObservableProperty]
    private string _commit;

    [RelayCommand]
    private void SendData(object obj)
    {
        var vm = App.Current.Services.GetService<SerialportPageVM>();

        var data = System.Text.Encoding.UTF8.GetBytes(Text);

        byte[] dataConvert;
        if (vm.serialPortHelper.IsOpen())
        {
            try
            {
                if (Hex)
                {
                    var temp = System.Text.Encoding.UTF8.GetString(data.ToArray());

                    var temp2 = temp.Replace(" ", "").Replace("\r\n", "");
                    dataConvert = StringHelper.HexStringToBytes(temp);
                }
                else
                {
                    dataConvert = data;
                }

                if (vm.SerialportUISetting.SendAndReceiveSettingVM.WithExtraEnter)
                {
                    var temp = dataConvert.ToList();
                    temp.Add(0x0d);
                    temp.Add(0x0a);
                    dataConvert = temp.ToArray();
                }
                vm.serialPortHelper.SendData(dataConvert);
            }
            catch (Exception ex)
            {
                MessageShowHelper.ShowErrorMessage($"{ResourceHelper.FindStringResource("ErrorSendFail")}\r\n" + ex.ToString());
                return;
            }
        }
    }

    public QuickSendModel ToM()
    {
        QuickSendModel quickSendVM = new QuickSendModel();
        quickSendVM.Id = Id;
        quickSendVM.Text = Text;
        quickSendVM.Hex = Hex;
        quickSendVM.Commit = Commit;

        return quickSendVM;
    }
}

public class QuickSendModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool Hex { get; set; }
    public string Commit { get; set; }

    public QuickSendVM ToVM()
    {
        QuickSendVM quickSendVM = new QuickSendVM();
        quickSendVM.Id = Id;
        quickSendVM.Text = Text;
        quickSendVM.Hex = Hex;
        quickSendVM.Commit = Commit;

        return quickSendVM;
    }
}