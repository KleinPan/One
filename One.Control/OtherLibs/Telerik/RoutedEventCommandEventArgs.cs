using System;
using System.Windows;

namespace One.Control.OtherLibs.Telerik
{
    internal sealed class RoutedEventCommandEventArgs : RadRoutedEventArgs
    {
        internal RoutedEventCommandEventArgs(RoutedEvent routedEvent, object parameter, bool closeAllEvent)
            : base(routedEvent)
        {
            this.CommandParameter = parameter;
            this.CloseAll = closeAllEvent;
        }

        protected override void InvokeEventHandler(Delegate genericHandler, object target)
        {
            var handler = (EventHandler<RoutedEventCommandEventArgs>)genericHandler;
            handler(target as DependencyObject, this);
        }

        public object CommandParameter
        {
            get;
            private set;
        }

        public bool CloseAll
        {
            get;
            private set;
        }
    }
}
