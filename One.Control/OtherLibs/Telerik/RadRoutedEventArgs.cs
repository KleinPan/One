using System.Windows;

namespace One.Control.OtherLibs.Telerik
{
    /// <summary>
    /// Represents the delegate for handlers that receive routed events.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances")]
    public delegate void RadRoutedEventHandler(object sender, RadRoutedEventArgs e);

    /// <summary>
    /// Contains state information and event data associated with a routed event.
    /// </summary>
    public class RadRoutedEventArgs : RoutedEventArgs
    {


        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class.
        /// </summary>
        public RadRoutedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class, 
        /// using the supplied routed event identifier. 
        /// </summary>
        /// <param name="routedEvent">
        /// The routed event identifier for this instance of the RoutedEventArgs class.
        /// </param>
        public RadRoutedEventArgs(RoutedEvent routedEvent)
            : this(routedEvent, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class, using 
        /// the supplied routed event identifier, and providing the opportunity 
        /// to declare a different source for the event. 
        /// </summary>
        /// <param name="routedEvent">
        /// The routed event identifier for this instance of the RoutedEventArgs class.
        /// </param>
        /// <param name="source">
        /// An alternate source that will be reported when the event is handled. 
        /// This pre-populates the Source property.
        /// </param>
        public RadRoutedEventArgs(RoutedEvent routedEvent, object source)

            : base(routedEvent, source)
        {

        }

        /// <summary>
        /// Initializes a new instance of the RadRoutedEventArgs class, using 
        /// the supplied routed event identifier, and providing the opportunity 
        /// to declare a different source for the event. 
        /// </summary>		
        /// <param name="source">
        /// An alternate source that will be reported when the event is handled. 
        /// This pre-populates the Source property.
        /// </param>
        public RadRoutedEventArgs(object source)

            : base(null, source)
        {

        }

    }
}
