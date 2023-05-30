using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;

using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Xml;

namespace One.Toolbox.Views
{
    /// <summary> SettingWindow.xaml 的交互逻辑 </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        //重载锁，防止逻辑卡死
        private static bool fileLoading = false;

        private static bool fileLoadingRev = false;

        //上次打开文件名
        private static string lastLuaFile = "";

        private static string lastLuaFileRev = "";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Tools.Global.setting;

            //重写关闭响应代码
            this.Closing += SettingWindow_Closing;

            //置顶显示以免被挡住
            this.Topmost = true;

            //初始化下拉框参数
            dataBitsComboBox.SelectedIndex = Tools.Global.setting.dataBits - 5;
            stopBitComboBox.SelectedIndex = Tools.Global.setting.stopBit - 1;
            dataCheckComboBox.SelectedIndex = Tools.Global.setting.parity;

            showHexComboBox.DataContext = Tools.Global.setting;

            //快速搜索
            SearchPanel.Install(textEditor.TextArea);
            SearchPanel.Install(textEditorRev.TextArea);
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".Lua.xshd";
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (System.IO.Stream s = assembly.GetManifestResourceStream(name))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    var xshd = HighlightingLoader.LoadXshd(reader);
                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
                    textEditorRev.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
                }
            }
            //加载上次打开的文件
            //loadLuaFile(Tools.Global.setting.sendScript);
            //loadLuaFileRev(Tools.Global.setting.recvScript);

            //加载编码
            var el = Encoding.GetEncodings();
            List<EncodingInfo> encodingList = new List<EncodingInfo>(el);
            //先排个序，美观点
            encodingList.Sort((x, y) => x.CodePage - y.CodePage);
            foreach (var en in encodingList)
            {
                ComboBoxItem c = new ComboBoxItem();
                c.Content = $"[{en.CodePage}] {en.Name}";
                c.Tag = en.CodePage;
                int index = encodingComboBox.Items.Add(c);
                if (Tools.Global.setting.encoding == en.CodePage)//现在用的编码
                    encodingComboBox.SelectedIndex = index;
            }
        }

        private void SettingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ////自动保存脚本
            //if (lastLuaFile != "")
            //    saveLuaFile(lastLuaFile);
            //if (lastLuaFileRev != "")
            //    saveLuaFileRev(lastLuaFileRev);
            if (Tools.Global.isMainWindowsClosed)
            {
                //说明软件关了
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;//取消这次关闭事件
                Hide();//隐藏窗口，以便下次调用show
            }
        }

        private void ApiDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start(Tools.Global.apiDocumentUrl);
        }

        private void OpenScriptFolderButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Tools.Global.GetTrueProfilePath() + "user_script_send_convert");
        }

        private void DataBitsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataBitsComboBox.SelectedItem != null)
            {
                Tools.Global.setting.dataBits = dataBitsComboBox.SelectedIndex + 5;
            }
        }

        private void StopBitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (stopBitComboBox.SelectedItem != null)
            {
                Tools.Global.setting.stopBit = stopBitComboBox.SelectedIndex + 1;
            }
        }

        private void DataCheckComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataCheckComboBox.SelectedItem != null)
            {
                Tools.Global.setting.parity = dataCheckComboBox.SelectedIndex;
                //Tools.MessageBox.Show((dataCheckComboBox.SelectedItem as ComboBoxItem).Content.ToString());
            }
        }

        private void NewScriptButton_Click(object sender, RoutedEventArgs e)
        {
            luaTestWrapPanel.Visibility = Visibility.Collapsed;
            newLuaFileWrapPanel.Visibility = Visibility.Visible;
        }

        private void TestScriptButton_Click(object sender, RoutedEventArgs e)
        {
            newLuaFileWrapPanel.Visibility = Visibility.Collapsed;
            luaTestWrapPanel.Visibility = Visibility.Visible;
        }

        private void LuaFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (luaFileList.SelectedItem != null && !fileLoading)
            //{
            //    if (lastLuaFile != "")
            //        saveLuaFile(lastLuaFile);
            //    string fileName = luaFileList.SelectedItem as string;
            //    loadLuaFile(fileName);
            //}
        }

        private void NewLuaFileCancelbutton_Click(object sender, RoutedEventArgs e)
        {
            newLuaFileWrapPanel.Visibility = Visibility.Collapsed;
        }

        private void NewLuaFilebutton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newLuaFileNameTextBox.Text))
            {
                Tools.MessageBox.Show(TryFindResource("LuaNoName") as string ?? "?!");
                return;
            }
            if (File.Exists(Tools.Global.ProfilePath + $"user_script_send_convert/{newLuaFileNameTextBox.Text}.lua"))
            {
                Tools.MessageBox.Show(TryFindResource("LuaExist") as string ?? "?!");
                return;
            }

            try
            {
                File.Create(Tools.Global.ProfilePath + $"user_script_send_convert/{newLuaFileNameTextBox.Text}.lua").Close();
                //loadLuaFile(newLuaFileNameTextBox.Text);
            }
            catch
            {
                Tools.MessageBox.Show(TryFindResource("LuaCreateFail") as string ?? "?!");
                return;
            }
            newLuaFileWrapPanel.Visibility = Visibility.Collapsed;
        }

        private void LuaTestbutton_Click(object sender, RoutedEventArgs e)
        {
            //if (luaFileList.SelectedItem != null && !fileLoading)
            //{
            //    try
            //    {
            //        byte[] r = LuaEnv.LuaLoader.Run($"{luaFileList.SelectedItem as string}.lua",
            //                            new System.Collections.ArrayList{"uartData",
            //                               Tools.Global.GetEncoding().GetBytes(luaTestTextBox.Text)});
            //        Tools.MessageBox.Show($"{TryFindResource("SettingLuaRunResult") as string ?? "?!"}\r\nHEX：" + Tools.Global.Byte2Hex(r) +
            //            $"\r\n{TryFindResource("SettingLuaRawText") as string ?? "?!"}" + Tools.Global.Byte2Readable(r));
            //    }
            //    catch (Exception ex)
            //    {
            //        Tools.MessageBox.Show($"{TryFindResource("ErrorScript") as string ?? "?!"}\r\n" + ex.ToString());
            //    }
            //}
        }

        private void LuaTestCancelbutton_Click(object sender, RoutedEventArgs e)
        {
            luaTestWrapPanel.Visibility = Visibility.Collapsed;
        }

        private void TextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            //自动保存脚本
            //if (lastLuaFile != "")
            //    saveLuaFile(lastLuaFile);
        }

        private void OpenLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", Tools.Global.GetTrueProfilePath() + "logs");
            }
            catch
            {
                Tools.MessageBox.Show($"尝试打开文件夹失败，请自行打开该路径：{Tools.Global.GetTrueProfilePath()}logs");
            }
        }

        private void encodingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            if ((int)((ComboBoxItem)c.SelectedItem).Tag == Tools.Global.setting.encoding)
                return;
            Tools.Global.setting.encoding = (int)((ComboBoxItem)c.SelectedItem).Tag;
        }

        private void luaFileListRev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (luaFileListRev.SelectedItem != null && !fileLoadingRev)
            //{
            //    if (lastLuaFileRev != "")
            //        saveLuaFileRev(lastLuaFileRev);
            //    string fileName = luaFileListRev.SelectedItem as string;
            //    loadLuaFileRev(fileName);
            //}
        }

        private void newScriptButtonRev_Click(object sender, RoutedEventArgs e)
        {
            luaTestWrapPanelRev.Visibility = Visibility.Collapsed;
            newLuaFileWrapPanelRev.Visibility = Visibility.Visible;
        }

        private void testScriptButtonRev_Click(object sender, RoutedEventArgs e)
        {
            newLuaFileWrapPanelRev.Visibility = Visibility.Collapsed;
            luaTestWrapPanelRev.Visibility = Visibility.Visible;
        }

        private void openScriptFolderButtonRev_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", Tools.Global.GetTrueProfilePath() + "user_script_recv_convert");
        }

        private void newLuaFilebuttonRev_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newLuaFileNameTextBoxRev.Text))
            {
                Tools.MessageBox.Show(TryFindResource("LuaNoName") as string ?? "?!");
                return;
            }
            if (File.Exists(Tools.Global.ProfilePath + $"user_script_recv_convert/{newLuaFileNameTextBoxRev.Text}.lua"))
            {
                Tools.MessageBox.Show(TryFindResource("LuaExist") as string ?? "?!");
                return;
            }

            try
            {
                File.Create(Tools.Global.ProfilePath + $"user_script_recv_convert/{newLuaFileNameTextBoxRev.Text}.lua").Close();
                //loadLuaFileRev(newLuaFileNameTextBoxRev.Text);
            }
            catch
            {
                Tools.MessageBox.Show(TryFindResource("LuaCreateFail") as string ?? "?!");
                return;
            }
            newLuaFileWrapPanelRev.Visibility = Visibility.Collapsed;
        }

        private void newLuaFileCancelbuttonRev_Click(object sender, RoutedEventArgs e)
        {
            newLuaFileWrapPanelRev.Visibility = Visibility.Collapsed;
        }

        private void luaTestbuttonRev_Click(object sender, RoutedEventArgs e)
        {
            //if (luaFileListRev.SelectedItem != null && !fileLoadingRev)
            //{
            //    try
            //    {
            //        byte[] r = LuaEnv.LuaLoader.Run(
            //            $"{luaFileListRev.SelectedItem as string}.lua",
            //            new System.Collections.ArrayList{
            //                "uartData",
            //                Tools.Global.GetEncoding().GetBytes(luaTestTextBoxRev.Text)
            //            },
            //            "user_script_recv_convert/");
            //        Tools.MessageBox.Show($"{TryFindResource("SettingLuaRunResult") as string ?? "?!"}\r\nHEX：" + Tools.Global.Byte2Hex(r) +
            //            $"\r\n{TryFindResource("SettingLuaRawText") as string ?? "?!"}" + Tools.Global.Byte2Readable(r));
            //    }
            //    catch (Exception ex)
            //    {
            //        Tools.MessageBox.Show($"{TryFindResource("ErrorScript") as string ?? "?!"}\r\n" + ex.ToString());
            //    }
            //}
        }

        private void luaTestCancelbuttonRev_Click(object sender, RoutedEventArgs e)
        {
            luaTestWrapPanelRev.Visibility = Visibility.Collapsed;
        }

        private void textEditorRev_LostFocus(object sender, RoutedEventArgs e)
        {
            //自动保存脚本
            //if (lastLuaFileRev != "")
            //    saveLuaFileRev(lastLuaFileRev);
        }
    }
}