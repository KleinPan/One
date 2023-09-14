using HandyControl.Tools.Extension;

using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace One.Toolbox.ViewModels.Dialogs
{
    public partial class DialogVMDynamic : DialogVMBase, IDialogResultable<Dictionary<string, string>>
    {
        public new Dictionary<string, string> Result { get; set; }=new Dictionary<string, string>();    

        public ObservableCollection<InputInfoVM> ShowClass { get; set; } = new ObservableCollection<InputInfoVM>();

        [ObservableProperty]
        private string errorInfo = " ";

        public DialogVMDynamic()
        {
        }

        private StackPanel spList;

        [RelayCommand]
        private void InitSPControl(object obj)
        {
            var args = obj as System.Windows.RoutedEventArgs;
            spList = args.OriginalSource as StackPanel;
        }

        public void IniDialog(List<InputInfoVM> inputInfoMs)
        {
            /*
            foreach (var item in inputInfoMs)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.Title;
                textBlock.Margin = new Thickness(5);

                spList.Children.Add(textBlock);

                TextBox textBox = new TextBox();
                textBox.Tag = item;
                textBox.KeyUp += TextBox_KeyUp;

                //var style = parentGrid.Resources["MyWatermarkTxb"];
                //textBox.SetValue(StyleProperty, style);

                //Binding binding = new Binding("Text.Length");
                //binding.Source = textBox;
                //textBox.SetBinding(WatermarkAttachedProperty.WatermarkProperty, binding);
                //spList.Children.Add(textBox);
            }
            */
            foreach (var item in inputInfoMs)
            {
                ShowClass.Add(item);
            }
        }

        public override void SureEvent()
        {
            foreach (var item in ShowClass)
            {
                Result.Add(item.Key, item.Content);
            }

            base.SureEvent();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var control = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                var type = (InputInfoVM)control.Tag;

                if (type.DataRule != null)
                {
                    var res = type.DataRule.IsMatch(control.Text.Trim());
                    if (!res)
                    {
                        ErrorInfo = $"Input not match the regex [{type.DataRule.ToString()}]!";
                        return;
                    }
                    else
                    {
                        ErrorInfo = "";
                    }
                }

                var index = spList.Children.IndexOf(control);

                if (spList.Children.Count > index + 2)
                {
                    var nextControl = spList.Children[index + 2];

                    nextControl.Focus();
                }

                else//关闭
                {
                    //judgeResult = GetInputInfo();
                    //this.DialogResult = judgeResult;
                    //this.Close();
                }
            }
            else
            {
            }
        }
    }
}