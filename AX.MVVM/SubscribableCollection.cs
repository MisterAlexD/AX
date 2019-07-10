using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Linq;
using AX.Common;

namespace AX.MVVM
{
    /// <summary>
    /// This collection never calls reset. So you can safely subscribe and unsubscribe to its items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SubscribableCollection<T> : ObservableCollection<T>, IList<T>, IReadOnlyObservableEnumerable<T>, IReadOnlyObservableCollection<T>
    {
        private bool suppressNotifications = false;

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
                OnItemRemove((T)e.OldItems[0]);
                OnItemAdd((T)e.NewItems[0]);
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!suppressNotifications)
                base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            var list = new List<T>(this);

            suppressNotifications = true;
            base.ClearItems();
            suppressNotifications = false;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            var index = this.Count - 1;
            suppressNotifications = true;

            foreach (T item in collection)
            {
                Add(item);
            }
            suppressNotifications = false;

            var list = new List<T>(collection);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list));
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            suppressNotifications = true;

            foreach (T item in collection)
            {
                Insert(index, item);
            }
            suppressNotifications = false;
            var list = new List<T>(collection);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, list, index));
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            suppressNotifications = true;

            foreach (T item in collection)
            {
                Remove(item);
            }

            suppressNotifications = false;
            var list = new List<T>(collection);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
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
