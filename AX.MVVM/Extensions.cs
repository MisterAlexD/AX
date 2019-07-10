using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;

namespace AX.MVVM
{
    public static class Extensions
    {
        public static bool Is(this PropertyChangedEventArgs e, string propName)
        {
            return e.PropertyName == propName;
        }

        public static bool IsAny(this PropertyChangedEventArgs e, params string[] propNames)
        {
            return propNames.Contains(e.PropertyName);
        }

        public static bool IsAny(this PropertyChangedEventArgs e, IEnumerable<string> propNames)
        {
            return propNames.Contains(e.PropertyName);
        }

        public static bool Is(this PropertyChangingEventArgs e, string propName)
        {
            return e.PropertyName == propName;
        }

        public static bool IsAny(this PropertyChangingEventArgs e, params string[] propNames)
        {
            return propNames.Contains(e.PropertyName);
        }

        public static bool IsAny(this PropertyChangingEventArgs e, IEnumerable<string> propNames)
        {
            return propNames.Contains(e.PropertyName);
        }

        public static void SubscribeCollectionItems<T>(this SubscribableCollection<T> collection, PropertyChangedEventHandler handler)
            where T : INotifyPropertyChanged
        {
            void collectionHandler(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.NewItems != null)
                    {
                        foreach (T item in e.NewItems)
                        {
                            item.PropertyChanged += handler;
                        }
                    }
                    if (e.OldItems != null)
                    {
                        foreach (T item in e.OldItems)
                        {
                            item.PropertyChanged -= handler;
                        }
                    }
                }
            }
            collection.CollectionChanged += collectionHandler;
        }

        public static void SubscribeCollectionItems<T>(this SubscribableCollection<T> collection, Action<T, PropertyChangedEventArgs> handler)
            where T : INotifyPropertyChanged
        {
            void itemsHandler(object item, PropertyChangedEventArgs e)
            {
                handler((T)item, e);
            }

            void collectionHandler(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (e.NewItems != null)
                    {
                        foreach (T item in e.NewItems)
                        {
                            item.PropertyChanged += itemsHandler;
                        }
                    }
                    if (e.OldItems != null)
                    {
                        foreach (T item in e.OldItems)
                        {
                            item.PropertyChanged -= itemsHandler;
                        }
                    }
                }
            }
            collection.CollectionChanged += collectionHandler;
        }
    }
}
