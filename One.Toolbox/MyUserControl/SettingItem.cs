namespace One.Toolbox.MyUserControl
{
    /// <summary> Button that opens a URL in a web browser. </summary>
    public class SettingItem : System.Windows.Controls.ContentControl
    {
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(SettingItem), new PropertyMetadata("DefaultHeader"));

        public string SubHeader
        {
            get { return (string)GetValue(SubHeaderProperty); }
            set { SetValue(SubHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubHeader. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubHeaderProperty =
            DependencyProperty.Register("SubHeader", typeof(string), typeof(SettingItem), new PropertyMetadata(""));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

         var contest=   this.Template.FindName("contentTest", this);
        }
    }
}