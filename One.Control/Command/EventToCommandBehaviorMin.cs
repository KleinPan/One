using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace One.Control.Command
{

    /*
    * 此类来源
    * https://stackoverflow.com/questions/6205472/mvvm-passing-eventargs-as-command-parameter/28410798#28410798?newreg=e1a56600c74f433693fd67eeff71a710
    */
    /// <summary> EventToCommand of MVVMLight 最小化 </summary>
    public class EventToCommandBehaviorMin : Behavior<FrameworkElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        // Event
        public string Event { get { return (string)GetValue(EventProperty); } set { SetValue(EventProperty, value); } }

        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string), typeof(EventToCommandBehaviorMin), new PropertyMetadata(null, OnEventChanged));

        // Command
        public ICommand Command { get { return (ICommand)GetValue(CommandProperty); } set { SetValue(CommandProperty, value); } }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehaviorMin), new PropertyMetadata(null));

        // PassArguments (default: false)
        public bool PassArguments { get { return (bool)GetValue(PassArgumentsProperty); } set { SetValue(PassArgumentsProperty, value); } }

        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool), typeof(EventToCommandBehaviorMin), new PropertyMetadata(false));

        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehaviorMin)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        protected override void OnAttached()
        {
            AttachHandler(this.Event); // initial set
        }

        /// <summary> Attaches the handler to the event </summary>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(this.AssociatedObject, _handler);

            // attach new event
            if (!string.IsNullOrEmpty(eventName))
            {
                EventInfo ei = this.AssociatedObject.GetType().GetEvent(eventName);
                if (ei != null)
                {
                    MethodInfo mi = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                    _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                    ei.AddEventHandler(this.AssociatedObject, _handler);
                    _oldEvent = ei; // store to detach in case the Event property changes
                }
                else
                    throw new ArgumentException(string.Format("The event '{0}' was not found on type '{1}'", eventName, this.AssociatedObject.GetType().Name));
            }
        }

        /// <summary> Executes the Command </summary>
        private void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = this.PassArguments ? e : null;
            if (this.Command != null)
            {
                if (this.Command.CanExecute(parameter))
                    this.Command.Execute(parameter);
            }
        }
    }
}
