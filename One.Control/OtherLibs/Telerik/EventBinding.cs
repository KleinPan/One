using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System;

namespace One.Control.OtherLibs.Telerik
{
    /// <summary>
    /// Represents a binding between an event and a command. The command is potentially a <see cref="RoutedCommand"/>. 
    /// </summary>
    public class EventBinding : Freezable, ICommandSource
    {

        /// <summary>
        /// Identifies the CommandParameter dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = InputBinding.CommandParameterProperty.AddOwner(typeof(EventBinding));

        /// <summary>
        /// Identifies the Command dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = InputBinding.CommandProperty.AddOwner(typeof(EventBinding));

        /// <summary>
        /// Identifies the CommandTarget dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = InputBinding.CommandTargetProperty.AddOwner(typeof(EventBinding));


        /// <summary>
        /// Identifies the RaiseOnHandledEvents dependency property.
        /// </summary>
        public static readonly DependencyProperty RaiseOnHandledEventsProperty =
            DependencyProperty.Register("RaiseOnHandledEvents", typeof(bool), typeof(EventBinding), new System.Windows.PropertyMetadata(false, OnRaiseOnHandledEventsChanged));

        /// <summary>
        /// Identifies the EventName dependency property.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventBinding), new System.Windows.PropertyMetadata(string.Empty, OnEventNameChanged));

        /// <summary>
        /// Identifies the PassEventArgsToCommand property.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsToCommandProperty =
            DependencyProperty.Register("PassEventArgsToCommand", typeof(bool), typeof(EventBinding), new PropertyMetadata(false));

        private static readonly MethodInfo methodInfo = typeof(EventBinding).GetMethod("OnEventTriggered", BindingFlags.Instance | BindingFlags.NonPublic);
        private EventInfo eventInfo;
        private Delegate eventHandler;
        private RoutedEvent routedEvent;
        private UIElement visual;

        /// <summary>
        /// Gets or sets the name of the event that will open the context menu.
        /// </summary>
        /// <value>The name of the event.</value>
        public string EventName
        {
            get
            {
                return (string)this.GetValue(EventNameProperty);
            }
            set
            {
                this.SetValue(EventNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="EventBinding"/> will raise the <see cref="Command"/> on handled routed events.
        /// The default value is false. This is a dependency property.
        /// </summary>
        public bool RaiseOnHandledEvents
        {
            get
            {
                return (bool)this.GetValue(RaiseOnHandledEventsProperty);
            }
            set
            {
                this.SetValue(RaiseOnHandledEventsProperty, value);
            }
        }

        /// <summary>
        ///  Gets or sets the <see cref="System.Windows.Input.ICommand" /> associated with this input binding.
        /// </summary>

        [Localizability(LocalizationCategory.NeverLocalize)]
        [TypeConverter(typeof(CommandConverter))]
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }
            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command-specific data for a particular command.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }
            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target element of the command.
        /// </summary>
        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)this.GetValue(CommandTargetProperty);
            }
            set
            {
                this.SetValue(CommandTargetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating if the event arguments will be passed to the command. If you specify
        /// CommandParameter this value is ignored.
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get
            {
                return (bool)this.GetValue(PassEventArgsToCommandProperty);
            }
            set
            {
                this.SetValue(PassEventArgsToCommandProperty, value);
            }
        }

        internal void SetSource(UIElement source)
        {
            this.RemoveHandler(this.visual, this.EventName);
            this.visual = source;
            this.AttachHandler(source);
        }


        /// <summary>
        /// Creates an instance of an <see cref="EventBinding"/>.
        /// </summary>
        /// <returns>A new instance of an <see cref="EventBinding"/>.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new EventBinding();
        }


        private static void OnEventNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventBinding eventBinding = d as EventBinding;
            eventBinding.RemoveHandler(eventBinding.visual, e.OldValue as string);
            eventBinding.AttachHandler(eventBinding.visual);
        }

        private static void OnRaiseOnHandledEventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EventBinding eventBinding = d as EventBinding;
            eventBinding.RemoveHandler(eventBinding.visual, eventBinding.EventName);
            eventBinding.AttachHandler(eventBinding.visual);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "eventName")]
        private void RemoveHandler(UIElement element, string eventName)
        {
            if (element != null)
            {

                if (this.routedEvent != null)
                {
                    element.RemoveHandler(this.routedEvent, new RoutedEventHandler(this.RoutedEventFired));
                }
                else if (this.eventInfo != null && this.eventHandler != null)
                {
                    this.eventInfo.RemoveEventHandler(element, this.eventHandler);
                    this.eventHandler = null;
                    this.eventInfo = null;
                }
            }
        }


        private void AttachHandler(UIElement element)
        {
            if (element != null)
            {
                string eventName = this.EventName;

#if WPF35
                bool isNullOrEmpty = string.IsNullOrEmpty(eventName);
#else
                bool isNullOrEmpty = string.IsNullOrWhiteSpace(eventName);
#endif
                if (!isNullOrEmpty)
                {
                    Type elementType = element.GetType();

                    this.routedEvent = GetRoutedEvent(elementType);
                    if (this.routedEvent != null)
                    {
                        element.AddHandler(this.routedEvent, new RoutedEventHandler(RoutedEventFired), this.RaiseOnHandledEvents);
                    }
                    else
                    {
                        this.AttachDynamicEventHandler(elementType);
                    }
                }
            }
        }

        private void AttachDynamicEventHandler(Type elementType)
        {
            this.eventInfo = elementType.GetEvent(this.EventName);
            if (this.eventInfo == null)
            {
                throw new InvalidOperationException(this.EventName + " event cannot be found.");
            }

            this.eventHandler = Delegate.CreateDelegate(this.eventInfo.EventHandlerType, this, methodInfo);
            this.eventInfo.AddEventHandler(this.visual, this.eventHandler);
        }

        private RoutedEvent GetRoutedEvent(Type elementType)
        {
            while (elementType != typeof(DependencyObject))
            {
                RoutedEvent[] routedEvents = EventManager.GetRoutedEventsForOwner(elementType);
                if (routedEvents != null)
                {
                    foreach (RoutedEvent routEvent in routedEvents)
                    {
                        if (routEvent.Name == this.EventName)
                        {
                            return routEvent;
                        }
                    }
                }

                elementType = elementType.BaseType;
            }

            return null;
        }

        private void OnEventTriggered(object sender, EventArgs e)
        {
            this.ExecuteCommand(e);
        }

        private void RoutedEventFired(object sender, RoutedEventArgs e)
        {
            this.ExecuteCommand(e);
        }

        private void ExecuteCommand(EventArgs e)
        {
            ICommand command = this.Command;
            if (command != null)
            {
                object commandParameter = this.CommandParameter;

                if (commandParameter == null && this.PassEventArgsToCommand)
                {
                    commandParameter = e;
                }

                IInputElement commandTarget = this.CommandTarget ?? this.visual as IInputElement;

                RoutedCommand routedCommand = command as RoutedCommand;
                if (routedCommand != null)
                {
                    if (routedCommand.CanExecute(commandParameter, commandTarget))
                    {
                        routedCommand.Execute(commandParameter, commandTarget);
                    }
                }
                else
                {
                    RoutedEventCommand eventCommand = command as RoutedEventCommand;
                    if (eventCommand != null)
                    {
                        if (eventCommand.CanExecute(commandParameter))
                        {
                            eventCommand.Execute(commandParameter, commandTarget);
                        }
                    }
                    else if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                }
            }
        }
    }
}
