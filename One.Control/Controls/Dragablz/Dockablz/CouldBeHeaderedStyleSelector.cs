﻿using System.Windows;
using System.Windows.Controls;

namespace One.Control.Controls.Dragablz.Dockablz
{
    public class CouldBeHeaderedStyleSelector : StyleSelector
    {
        public Style NonHeaderedStyle { get; set; }

        public Style HeaderedStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return container is HeaderedDragablzItem || container is HeaderedContentControl
                ? HeaderedStyle
                : NonHeaderedStyle;
        }
    }
}