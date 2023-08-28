using System.Windows;

namespace One.Control.Controls.Dragablz
{
    public interface INewTabHost<out TElement> where TElement : UIElement
    {
        TElement Container { get; }
        TabablzControl TabablzControl { get; }
    }
}