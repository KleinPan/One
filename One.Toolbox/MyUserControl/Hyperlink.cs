using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace One.Toolbox.MyUserControl
{
    /// <summary> Button that opens a URL in a web browser. </summary>
    public class Hyperlink : Button
    {
        /// <summary> Property for <see cref="NavigateUri"/>. </summary>
        public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register(nameof(NavigateUri),
            typeof(string), typeof(Hyperlink), new PropertyMetadata(string.Empty));

        /// <summary> The URL (or application shortcut) to open. </summary>
        public string NavigateUri
        {
            get => GetValue(NavigateUriProperty) as string ?? string.Empty;
            set => SetValue(NavigateUriProperty, value);
        }

        protected override void OnClick()
        {
            RoutedEventArgs newEvent = new RoutedEventArgs(ButtonBase.ClickEvent, this);
            RaiseEvent(newEvent);

            if (newEvent.Handled || string.IsNullOrEmpty(NavigateUri))
                return;

            try
            {
                Debug.WriteLine($"INFO | Hyperlink clicked, with href: {NavigateUri}", "Wpf.Ui.Hyperlink");

                ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
                {
                    UseShellExecute = true
                };

                Process.Start(sInfo);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}