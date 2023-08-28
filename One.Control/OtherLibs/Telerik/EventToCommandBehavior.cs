using System.Windows;

namespace One.Control.OtherLibs.Telerik
{
    /// <summary> Behavior that execute command when given event is raised. </summary>
    public static class EventToCommandBehavior
    {
        private static readonly DependencyProperty EventBindingsProperty =
            DependencyProperty.RegisterAttached("EventBindingsInternal", typeof(EventBindingCollection), typeof(EventToCommandBehavior), new System.Windows.PropertyMetadata(null, OnEventBindingsPropertyChanged));

        /// <summary> Gets the collection of <see cref="EventBinding"/> s associated with this element. </summary>
        /// <param name="obj"> The object that <see cref="EventBindingCollection"/> is returned. </param>
        /// <returns> Returns the <see cref="EventBindingCollection"/> associated with this object. </returns>
        public static EventBindingCollection GetEventBindings(DependencyObject obj)
        {
            var collection = (EventBindingCollection)obj.GetValue(EventBindingsProperty);
            if (collection == null)
            {
                collection = new EventBindingCollection();
                obj.SetValue(EventBindingsProperty, collection);
            }

            return collection;
        }

        private static void OnEventBindingsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This is possible in WPF because Freezable raise property change for same values.
            if (e.OldValue == e.NewValue)
            {
                return;
            }

            var collection = e.OldValue as EventBindingCollection;
            if (collection != null)
            {
                collection.SetOwner(null);
            }

            collection = e.NewValue as EventBindingCollection;
            if (collection != null)
            {
                collection.SetOwner(d as UIElement);
            }
        }
    }
}