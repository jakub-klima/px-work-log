using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PxWorkLog.Core.Utils
{
    public class CollectionEventAggregator
    {
        public event EventHandler ItemChanged;

        public CollectionEventAggregator()
        {
        }

        public void Initialize<TItem>(ObservableCollection<TItem> collection, Action<TItem> subscriber, Action<TItem> unsubscriber)
        {
            Initialize(collection, collection, subscriber, unsubscriber);
        }

        public void Initialize<TItem>(ProjectedObservableCollection<TItem> collection, Action<TItem> subscriber, Action<TItem> unsubscriber)
        {
            Initialize(collection, collection, subscriber, unsubscriber);
        }

        private void Initialize<TItem>(IEnumerable<TItem> collection, INotifyCollectionChanged collectionChanged, Action<TItem> subscriber, Action<TItem> unsubscriber)
        {
            var registeredItems = new List<TItem>();

            RegisterItems();

            collectionChanged.CollectionChanged += (sender, e) =>
            {
                UnregisterItems();
                RegisterItems();
            };

            void RegisterItems()
            {
                foreach (TItem item in collection)
                {
                    subscriber(item);
                }
                registeredItems.AddRange(collection);
            }

            void UnregisterItems()
            {
                foreach (TItem item in registeredItems)
                {
                    unsubscriber(item);
                }
                registeredItems.Clear();
            }
        }

        public void RaiseItemChanged(object sender, EventArgs e)
        {
            ItemChanged?.Invoke(sender, e);
        }
    }
}
