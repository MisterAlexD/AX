using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    /// <summary>
    /// This collection never calls reset. So you can safely subscribe and unsubscribe to its items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SubscribableCollection<T> : ObservableCollection<T>, IList<T>, IObservableEnumerable<T>, IReadOnlyObservableCollection<T>
    {
        private Action<T> OnItemAdd = null;
        private Action<T> OnItemRemove = null;

        public SubscribableCollection()
        {            
            CollectionChanged += AxObservableCollection_CollectionChanged;
        }

        public SubscribableCollection(Action<T> onItemAdd, Action<T> onItemRemove)
        : this()
        {
            this.OnItemAdd = onItemAdd;
            this.OnItemRemove = onItemRemove;
        }

        private void AxObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && OnItemAdd != null)
            {
                e.NewItems?.Cast<T>().ForEach(x => OnItemAdd(x));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && OnItemRemove != null)
            {
                e.OldItems?.Cast<T>().ForEach(x => OnItemRemove(x));
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                OnItemRemove?.Invoke((T)e.OldItems[0]);
                OnItemAdd?.Invoke((T)e.NewItems[0]);
            }
        }

        protected override void ClearItems()
        {
            var list = new List<T>(this);
            RemoveRange(list);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            foreach(var item in collection)
            {
                Add(item);
            }
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T item in collection.ToArray().Reverse<T>())
            {
                Insert(index, item);
            }
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T item in collection.ToArray())
            {
                Remove(item);
            }

        }

        public void ReplaceWhole(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            Clear();

            AddRange(collection);
        }
    }
}
