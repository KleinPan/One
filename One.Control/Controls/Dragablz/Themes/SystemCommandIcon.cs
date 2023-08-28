using System.Windows;

namespace One.Control.Controls.Dragablz.Themes
{
    public enum SystemCommandType
    {
        CloseWindow,
        MaximizeWindow,
        MinimzeWindow,
        RestoreWindow
    }

    public class SystemCommandIcon : System.Windows.Controls.Control
    {
        static SystemCommandIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SystemCommandIcon), new FrameworkPropertyMetadata(typeof(SystemCommandIcon)));
        }

        public static readonly DependencyProperty SystemCommandTypeProperty = DependencyProperty.Register(
            "SystemCommandType", typeof(SystemCommandType), typeof(SystemCommandIcon), new PropertyMetadata(default(SystemCommandType)));

        public SystemCommandType SystemCommandType
        {
            get { return (SystemCommandType)GetValue(SystemCommandTypeProperty); }
            set { SetValue(SystemCommandTypeProperty, value); }
        }
    }
}