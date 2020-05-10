using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace PxWorkLog.Core.Utils
{
    public class ProjectedObservableCollection<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        private readonly ObservableCollection<T> collection;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => collection.CollectionChanged += value;
            remove => collection.CollectionChanged -= value;
        }

        private ProjectedObservableCollection(ObservableCollection<T> collection)
        {
            this.collection = collection;
        }

        public static ProjectedObservableCollection<T> Create<TSourceItem>(ObservableCollection<TSourceItem> source, Func<TSourceItem, T> projector)
        {
            var collection = new ObservableCollection<T>(source.Select(projector));

            source.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        TSourceItem addedItem = source[e.NewStartingIndex];
                        collection.Add(projector(addedItem));
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        collection.RemoveAt(e.OldStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Reset:
                    default:
                        throw new NotImplementedException();
                }
            };

            return new ProjectedObservableCollection<T>(collection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
