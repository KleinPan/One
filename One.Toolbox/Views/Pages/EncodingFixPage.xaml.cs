using System.Text;
using System.Windows.Controls;

namespace One.Toolbox.Views.Pages
{
    /// <summary> EncodingFixPage.xaml 的交互逻辑 </summary>
    public partial class EncodingFixPage : Page
    {
        public EncodingFixPage()
        {
            InitializeComponent();
        }

        private class fixedData
        {
            public string raw { get; set; }
            public string target { get; set; }
            public string result { get; set; }
        }

        private string[] encodingList = new string[]
        {
            "UTF-8",
            "GBK",
            "windows-1252",
            "Big5",
            "Shift_Jis",
            "iso-8859-1",
        };

        private void RawTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FixResultList.Items.Clear();
            for (int i = 0; i < encodingList.Length; i++)
            {
                for (int j = 0; j < encodingList.Length; j++)
                {
                    if (i == j)
                        continue;
                    FixResultList.Items.Add(new fixedData
                    {
                        raw = encodingList[i],
                        target = encodingList[j],
                        result = Encoding.GetEncoding(encodingList[i]).GetString(Encoding.GetEncoding(encodingList[j]).GetBytes(RawTextBox.Text))
                    });
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FixResultList.SelectedItem == null) return;
                //获取单元格内容
                string copiedData = (FixResultList.SelectedItem as fixedData).result;
                if (string.IsNullOrEmpty(copiedData)) return;
                //复制到剪贴板
                Clipboard.Clear();
                Clipboard.SetData(DataFormats.Text, copiedData);
                Tools.MessageBox.Show("copyed:\r\n" + copiedData);
            }
            catch (Exception ee)
            {
                Tools.MessageBox.Show("error:\r\n" + ee.Message);
            }
        }
    }
}