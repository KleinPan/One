using System.Windows.Controls;

namespace One.Control.Controls.Dragablz
{
    public class HorizontalOrganiser : StackOrganiser
    {
        public HorizontalOrganiser() : base(Orientation.Horizontal)
        { }

        public HorizontalOrganiser(double itemOffset) : base(Orientation.Horizontal, itemOffset)
        { }
    }
}