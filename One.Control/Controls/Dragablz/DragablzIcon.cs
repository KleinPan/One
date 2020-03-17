using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace One.Control.Controls.Dragablz
{
    public class DragablzIcon : System.Windows.Controls.Control
    {
        static DragablzIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragablzIcon), new FrameworkPropertyMetadata(typeof(DragablzIcon)));
        }
    }
}
