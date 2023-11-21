using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace One.Control.Controls.RichTextboxEx
{
    [TemplatePart(Name = "PART_StackPanel", Type = typeof(StackPanel))]
    public class RichTextboxEx : RichTextBox
    {
        public bool ShowLineNumber
        {
            get { return (bool)GetValue(ShowLineNumberProperty); }
            set { SetValue(ShowLineNumberProperty, value); }
        }

        /// <summary> 行号 </summary>
        public static readonly DependencyProperty ShowLineNumberProperty =
            DependencyProperty.Register("ShowLineNumber", typeof(bool), typeof(RichTextboxEx), new PropertyMetadata(false));

        static RichTextboxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextboxEx), new FrameworkPropertyMetadata(typeof(RichTextboxEx)));
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            LineNumbers();
        }

        private void LineNumbers()
        {
            if (!ShowLineNumber)
            {
                return;
            }

            try
            {
                if (stackPanel == null)
                {
                    return;
                }
                stackPanel.Children.Clear();
                int lineNumber = 1;
                var lineStartPos = Document.ContentStart.GetLineStartPosition(0);

                while (true)
                {
                    Label label = new();

                    //var leftRect = lineStartPos.GetCharacterRect(LogicalDirection.Forward);
                    var rightRect = lineStartPos.GetCharacterRect(LogicalDirection.Backward);

                    label.Height = rightRect.Height;
                    label.Content = lineNumber.ToString();
                    stackPanel.Children.Add(label);

                    int result;
                    lineStartPos = lineStartPos.GetLineStartPosition(1, out result);

                    if (result == 0)
                    {
                        break;
                    }
                    ++lineNumber;
                }
            }
            catch (Exception)
            {
            }
        }

        public StackPanel stackPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            stackPanel = GetTemplateChild("PART_StackPanel") as StackPanel;
        }
    }
}