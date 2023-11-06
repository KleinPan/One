using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace One.Control.Controls.Panels;

/// <summary> 当不需要使用Grid的分行分列，则可使用 OverlapPanel,仅仅好像是提高了效率 </summary>
public class OverlapPanel : Panel
{
    /// <summary> 测量子元素在布局中所需的大小 </summary>
    /// <param name="constraint"> 此元素可提供给子元素的可用大小。 可指定无穷大作为一个值，该值指示元素将调整到适应内容的大小。 </param>
    /// <returns> 此元素基于其对子元素大小的计算确定它在布局期间所需要的大小。 </returns>
    protected override Size MeasureOverride(Size constraint)
    {
        // do something with child.DesiredSize, either sum them directly or apply whatever logic your element has for reinterpreting the child sizes

        // if greater than availableSize, must decide what to do and which size to return

        Size panelDesiredSize = new Size();
        UIElementCollection children = InternalChildren;

        for (int i = 0, count = children.Count; i < count; ++i)
        {
            UIElement child = children[i];
            if (child != null)
            {
                child.Measure(constraint);
                panelDesiredSize.Width = Math.Max(panelDesiredSize.Width, child.DesiredSize.Width);
                panelDesiredSize.Height = Math.Max(panelDesiredSize.Height, child.DesiredSize.Height);
            }
        }
        return (panelDesiredSize);
    }

    /// <summary> </summary>
    /// <param name="arrangeSize">
    /// The final area within the parent that this element should use to arrange itself and its children.
    /// <para> 父元素中的最后一个区域，此元素应用于排列自身及其子元素。 </para>
    /// </param>
    /// <returns> The actual size used.实际使用的大小。 </returns>
    protected override Size ArrangeOverride(Size arrangeSize)
    {
        //Parent elements should call Arrange(Rect) on each child, otherwise the child elements will not be rendered.
        UIElementCollection children = InternalChildren;
        for (int i = 0, count = children.Count; i < count; ++i)
        {
            UIElement child = children[i];
            if (child != null)
            {
                child.Arrange(new Rect(arrangeSize));
            }
        }
        return (arrangeSize);
    }
}