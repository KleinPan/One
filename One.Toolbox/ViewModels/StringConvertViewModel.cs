using One.Toolbox.Helpers;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Shapes;

using TextBlock = System.Windows.Controls.TextBlock;

namespace One.Toolbox.ViewModels
{
    public partial class StringConvertViewModel : BaseViewModel
    {
        public ObservableCollection<string> ConverterTaskList { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedConverterTask;

        [ObservableProperty]
        private string leftSelectedConverterTask;

        public ObservableCollection<string> SelectedConverterTaskList { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string inputString;

        [ObservableProperty]
        private string outputString;

        [ObservableProperty]
        private double lineHeight;

        public override void InitializeViewModel()
        {
            foreach (var i in converters)
                ConverterTaskList.Add(i.Key);

            base.InitializeViewModel();
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            ConfigHelper.Instance.Load();
        }

        private void DoConvert()
        {
            if (string.IsNullOrEmpty(InputString))
                return;

            byte[] row = Encoding.Default.GetBytes(InputString);
            for (int i = 0; i < SelectedConverterTaskList.Count; i++)
            {
                string name = SelectedConverterTaskList[i] as string;
                if (converters.ContainsKey(name))
                    row = converters[name](row);
            }
            OutputString = Encoding.Default.GetString(row);
        }

        private void DoConvert2()
        {
            if (string.IsNullOrEmpty(InputString))
                return;

            byte[] row = Encoding.Default.GetBytes(InputString);

            if (converters.ContainsKey(SelectedConverterTask))
                row = converters[SelectedConverterTask](row);

            OutputString = Encoding.Default.GetString(row);
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        private void AddNewTask()
        {
            if (SelectedConverterTask == null)
                return;
            SelectedConverterTaskList.Add(SelectedConverterTask);
            DoConvert();
        }

        [RelayCommand]
        private void DeleceLastTask()
        {
            if (SelectedConverterTaskList.Count == 0)
                return;

            if (string.IsNullOrEmpty(LeftSelectedConverterTask))
            {
                return;
            }
            SelectedConverterTaskList.Remove(LeftSelectedConverterTask);
            DoConvert();
        }

        [RelayCommand]
        private void ExcuteSelectedOme()
        {
            if (SelectedConverterTask == null)
                return;
            LineHeight = 0.0;

            if (wrapPanel != null)
            {
                wrapPanel.Children.Clear();
            }

            DoConvert2();
        }

        /// <summary> 转换器 </summary>
        private Dictionary<string, Func<byte[], byte[]>> converters = new Dictionary<string, Func<byte[], byte[]>>
        {
            ["InitToHexArray"] = (e) => Encoding.Default.GetBytes(InitArrayEvent(Encoding.Default.GetString(e))),
            ["String to Hex(with space)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(e).Replace("-", " ")),
            ["String to Hex(without space)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(e).Replace("-", "")),
            ["Hex to String"] = (e) => Hex2byte(Encoding.Default.GetString(e)),
            ["String to Base64"] = (e) => { try { return Encoding.Default.GetBytes(System.Convert.ToBase64String(e)); } catch (Exception ee) { return Encoding.Default.GetBytes(ee.Message); } },
            ["Base64 to String"] = (e) => { try { return (System.Convert.FromBase64String(Encoding.Default.GetString(e))); } catch (Exception ee) { return Encoding.Default.GetBytes(ee.Message); } },
            ["URL encode"] = (e) => Encoding.Default.GetBytes(System.Web.HttpUtility.UrlEncode(Encoding.Default.GetString(e))),
            ["URL decode"] = (e) => Encoding.Default.GetBytes(System.Web.HttpUtility.UrlDecode(Encoding.Default.GetString(e))),
            ["HTML encode"] = (e) => Encoding.Default.GetBytes(System.Web.HttpUtility.HtmlEncode(Encoding.Default.GetString(e))),
            ["HTML decode"] = (e) => Encoding.Default.GetBytes(System.Web.HttpUtility.HtmlDecode(Encoding.Default.GetString(e))),
            ["String to Unicode"] = (e) => Encoding.Default.GetBytes(String2Unicode(Encoding.Default.GetString(e))),
            ["Unicode to String"] = (e) => Encoding.Default.GetBytes(Unicode2String(Encoding.Default.GetString(e))),
            ["String to MD5 (Hex)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(MD5Encrypt(e)).Replace("-", "")),
            ["String to SHA-1 (Hex)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(Sha1Encrypt(e)).Replace("-", "")),
            ["String to SHA-256 (Hex)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(Sha256Encrypt(e)).Replace("-", "")),
            ["String to SHA-512 (Hex)"] = (e) => Encoding.Default.GetBytes(BitConverter.ToString(Sha512Encrypt(e)).Replace("-", "")),
        };

        #region Helpers

        private static string InitArrayEvent(string input)
        {
            try
            {
                string output = "";

                if (input.Contains(' '))
                {
                    input = input.Replace(" ", "");
                }
                else
                {
                }

                int len = input.Length;

                for (int i = 0; i < len; i = i + 2)
                {
                    if (i > input.Length)
                    {
                        break;
                    }

                    var single = "0x" + input[i] + input[i + 1] + ",";
                    output += single;
                }

                return output;
            }
            catch (Exception ex)
            {
                MessageShowHelper.ShowErrorMessage(ex.Message);
                return "";
            }
        }

        public static byte[] Hex2byte(string mHex)
        {
            mHex = Regex.Replace(mHex, "[^0-9A-Fa-f]", "");
            if (mHex.Length % 2 != 0)
                mHex = mHex.Remove(mHex.Length - 1, 1);
            if (mHex.Length <= 0) return new byte[] { };
            byte[] vBytes = new byte[mHex.Length / 2];
            for (int i = 0; i < mHex.Length; i += 2)
                if (!byte.TryParse(mHex.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                    vBytes[i / 2] = 0;
            return vBytes;
        }

        public static byte[] MD5Encrypt(byte[] b)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(b);
            return hashedDataBytes;
        }

        public static byte[] Sha1Encrypt(byte[] b)
        {
            SHA1CryptoServiceProvider md5Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(b);
            return hashedDataBytes;
        }

        public static byte[] Sha256Encrypt(byte[] b)
        {
            SHA256CryptoServiceProvider md5Hasher = new SHA256CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(b);
            return hashedDataBytes;
        }

        public static byte[] Sha512Encrypt(byte[] b)
        {
            SHA512CryptoServiceProvider md5Hasher = new SHA512CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(b);
            return hashedDataBytes;
        }

        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => System.Convert.ToChar(System.Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }

        #endregion Helpers

        #region ShowIndex

        private WrapPanel wrapPanel;

        [RelayCommand]
        private void ShowIndexEvent(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(InputString)) { return; }

                wrapPanel = (WrapPanel)obj;
                if (wrapPanel == null) { return; }

                var input = InputString.Trim();
                OutputString = "";
                LineHeight = 43.0;
                //归一
                if (input.Contains(' '))
                {
                    input = input.Replace(" ", "");
                }

                int len1 = input.Length;

                int test = 0;
                for (int i = 0; i < len1; i += 2)
                {
                    if (i > input.Length)
                    {
                        break;
                    }
                    string single = "";

                    single = input[i].ToString() + input[i + 1].ToString() + " ";

                    OutputString += single;
                }
                OutputString = OutputString.TrimEnd();

                GenerateBox(wrapPanel, len1 / 2);
            }
            catch (Exception ex)
            {
                MessageShowHelper.ShowErrorMessage(ex.Message);
            }
        }

        private int chahacterCount = 49;

        private void GenerateBox(object element, int count)
        {
            WrapPanel wrapPanel = (WrapPanel)element;
            wrapPanel.Children.Clear();

            for (int i = 0; i < count; i++)
            {
                bool isFirstLine = i < chahacterCount ? true : false;

                GenTarget(wrapPanel, isFirstLine, i);
            }
        }

        private void GenTarget(WrapPanel wrapPanel, bool isFirstRow, int realIndex)
        {
            Border border = new Border();

            border.BorderThickness = new Thickness(1);
            //border.BorderBrush = System.Windows.Media.Brushes.Black;

            Grid innerGrid = new Grid();

            innerGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            innerGrid.RowDefinitions.Add(new RowDefinition());

            double marginWidth = 1.55;
            double marginUp, marginDown = 0;
            if (isFirstRow)
            {
                marginUp = 28;
            }
            else
            {
                marginUp = 21;
            }

            //每行数量

            int index = realIndex % chahacterCount;
            if (index == 0)
            {
                //border.BorderBrush = System.Windows.Media.Brushes.Red;
                border.Margin = new System.Windows.Thickness(13, marginUp, marginWidth, marginDown);
            }
            else if (index == chahacterCount)
            {
                //border.BorderBrush = System.Windows.Media.Brushes.Blue;
                border.Margin = new System.Windows.Thickness(1, marginUp, 0, marginDown);
            }
            else
            {
                border.Margin = new System.Windows.Thickness(marginWidth, marginUp, marginWidth, marginDown);
            }

            innerGrid.Width = 18;
            innerGrid.Height = 20;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = realIndex.ToString();
            textBlock.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            textBlock.VerticalAlignment = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //textBlock.Foreground = System.Windows.Media.Brushes.Gray;
            //textBlock.Foreground = Wpf.Ui.Appearance.ApplicationAccentColorManager.SecondaryAccentBrush; //Wpf.Ui.Appearance.Accent.SecondaryAccentBrush;
            innerGrid.Children.Add(textBlock);
            Grid.SetRow(textBlock, 1);

            Line line = new();
            line.X1 = 2;
            line.Y1 = 0;
            line.X2 = 16;
            line.Y2 = 0;
            line.Stroke = System.Windows.Media.Brushes.Black;

            innerGrid.Children.Add(line);
            Grid.SetRow(line, 0);

            border.Child = innerGrid;

            wrapPanel.Children.Add(border);
        }

        #endregion ShowIndex
    }
}