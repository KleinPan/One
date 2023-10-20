using Microsoft.Extensions.DependencyInjection;

using One.Core.Helpers.DataProcessHelpers;
using One.Toolbox.Helpers;

namespace One.Toolbox.ViewModels.Serialport;

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

    [RelayCommand]
    private void SendData(object obj)
    {
        var vm = App.Current.Services.GetService<SerialportViewModel>();

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

                if (vm.SerialportUISetting.WithExtraEnter)
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
}