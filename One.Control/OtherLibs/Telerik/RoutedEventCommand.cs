using System;
using System.Windows;
using System.Windows.Input;

namespace One.Control.OtherLibs.Telerik
{
    internal class RoutedEventCommand : ICommand
    {
        private RoutedEvent routedEvent;
        private bool closeAllEvent;
        internal RoutedEventCommand(RoutedEvent targetRoutedEvent, bool closeAll)
        {
            this.routedEvent = targetRoutedEvent;
            this.closeAllEvent = closeAll;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.Execute(parameter, FilterInputElement(GetFocusedElement()));
        }

        public void Execute(object parameter, IInputElement target)
        {
            if (target != null && !IsValid(target))
            {
                throw new InvalidOperationException("Invalid target.");
            }

            if (target == null)
            {
                target = FilterInputElement(GetFocusedElement());
            }

            if (target != null)
            {
                target.RaiseEvent(new RoutedEventCommandEventArgs(this.routedEvent, parameter, this.closeAllEvent));
            }
        }

        private static IInputElement FilterInputElement(IInputElement element)
        {
            return element != null && IsValid(element) ? element : null;
        }

        private static IInputElement GetFocusedElement()
        {

            return Keyboard.FocusedElement;

        }

        private static bool IsValid(IInputElement e)
        {

            return e is UIElement || e is ContentElement || e is UIElement3D;

        }
    }
}
