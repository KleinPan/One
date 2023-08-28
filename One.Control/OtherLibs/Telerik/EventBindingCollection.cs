using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace One.Control.OtherLibs.Telerik
{
    public class EventBindingCollection : FreezableCollection<EventBinding>

    {
        private UIElement owner;
        private List<EventBinding> eventBindingsCopy;

        /// <summary> Initializes a new instance of the <see cref="EventBindingCollection"/> class. </summary>
        public EventBindingCollection()
        {
            this.eventBindingsCopy = new List<EventBinding>();
            ((INotifyCollectionChanged)this).CollectionChanged += OnEventBindingCollectionChanged;
        }

        internal void SetOwner(UIElement sourceElement)
        {
            this.owner = sourceElement;
            foreach (var binding in this.eventBindingsCopy)
            {
                binding.SetSource(sourceElement);
            }
        }

        /// <summary> Creates new instance of <see cref="EventBindingCollection"/>. </summary>
        /// <returns> New instance of <see cref="EventBindingCollection"/>. </returns>
        protected override Freezable CreateInstanceCore()
        {
            return new EventBindingCollection();
        }

        private void OnEventBindingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (EventBinding item in e.NewItems)
                        {
                            this.AddInputBinding(item);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (EventBinding item in e.OldItems)
                        {
                            this.RemoveInputBinding(item);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    this.RemoveInputBinding(e.OldItems[0] as EventBinding);
                    this.AddInputBinding(e.NewItems[0] as EventBinding);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in this.eventBindingsCopy)
                    {
                        this.RemoveInputBinding(item);
                    }
                    this.eventBindingsCopy.Clear();
                    break;

                default:
                    break;
            }
        }

        private void AddInputBinding(EventBinding eventBinding)
        {
            if (eventBinding != null)
            {
                this.eventBindingsCopy.Add(eventBinding);
                eventBinding.SetSource(this.owner);
            }
        }

        private void RemoveInputBinding(EventBinding eventBinding)
        {
            if (eventBinding != null)
            {
                this.eventBindingsCopy.Remove(eventBinding);
                eventBinding.SetSource(null);
            }
        }
    }
}