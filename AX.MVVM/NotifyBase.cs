using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


namespace AX.MVVM
{
    public abstract partial class NotifyBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        //property, properties that depends on it
        private static Dictionary<string, List<string>> _dependentProperties = new Dictionary<string, List<string>>();

        public NotifyBase()
        {
            PropertyChanged += ViewModelBase_PropertyChanged;
        }

        /// <summary>
        /// Every time dependence property will fire 'PropertyChanged', subscriber will do too
        /// </summary>
        /// <param name="subscriberName">Property which depends on other property</param>
        /// <param name="dependenceName"></param>
        protected static void SubscribeProperty(string subscriberName, string dependenceName)
        {
            if (subscriberName == null || dependenceName == null)
            {
                throw new ArgumentNullException();
            }

            if (_dependentProperties.ContainsKey(dependenceName))
            {
                var subscribers = _dependentProperties[dependenceName];
                if (!subscribers.Contains(subscriberName))
                {
                    subscribers.Add(subscriberName);
                }
            }
            else
            {
                _dependentProperties.Add(dependenceName, new List<string>() { subscriberName });
            }
        }

        /// <summary>
        /// Every time any of dependence properties will fire 'PropertyChanged', subscriber will do too
        /// </summary>
        /// <param name="subscriberName">Property which depends on other properties</param>
        /// <param name="dependenceNames"></param>
        protected static void SubscribeProperty(string subscriberName, params string[] dependenceNames)
        {
            foreach (var dependence in dependenceNames)
            {
                SubscribeProperty(subscriberName, dependence);
            }
        }

        private List<string> _allreadyCalled = new List<string>();
        private int _recursionCounter = 0;
        private void ViewModelBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _recursionCounter += 1;
            _allreadyCalled.Add(e.PropertyName);
            if (_dependentProperties.ContainsKey(e.PropertyName))
            {
                var subscribers = _dependentProperties[e.PropertyName];
                foreach (var subscriber in subscribers)
                {
                    if (!_allreadyCalled.Contains(subscriber))
                    {
                        OnPropertyChanged(subscriber);
                    }
                }
            }
            _recursionCounter -= 1;
            if (_recursionCounter == 0)
                _allreadyCalled.Clear();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName]string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Checks that new value are not equal to the old one, and if it's not - sets it and call 'PropertyChanged'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            OnPropertyChanging(propertyName);
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Checks that new value are not equal to the old one, and if it's not - sets it and call 'PropertyChanged'
        /// Also subsribes to all 'PropertyChanged' calls of new value and retranslates them. Those will be called with 'propertyName.' prefix
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected virtual bool SetAndSubscribe<T>(ref T storage, T value, bool callProperty = false, [CallerMemberName] string propertyName = "")
            where T : INotifyPropertyChanged
        {
            void handler(object x, PropertyChangedEventArgs y)
            {
                OnPropertyChanged($"{propertyName}.{y.PropertyName}");
                if (callProperty)
                    OnPropertyChanged(propertyName);
            }

            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            if (storage != null)
            {
                value.PropertyChanged -= handler;
            }
            OnPropertyChanging(propertyName);
            storage = value;
            if (value != null)
            {
                value.PropertyChanged += handler;
            }
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual bool RefillCollection<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
            where T : IList
        {
            if (EqualityComparer<T>.Default.Equals(storage, value) || value == null)
            {
                return false;
            }
            else
            {
                OnPropertyChanging(propertyName);
                if (storage == null)
                {
                    storage = value;
                }
                else
                {
                    storage.Clear();
                    foreach (var item in value)
                    {
                        storage.Add(item);
                    }
                }
                OnPropertyChanged(propertyName);
                return true;
            }
        }

        protected virtual bool RefillCollection<T>(ref SubscribableCollection<T> storage, IEnumerable<T> value, [CallerMemberName] string propertyName = "")
        {
            if (storage == value || value == null)
            {
                return false;
            }
            else
            {
                OnPropertyChanging(propertyName);
                if (storage == null)
                {
                    if (value is SubscribableCollection<T> initCollection)
                    {
                        storage = initCollection;
                    }
                    else
                    {
                        storage = new SubscribableCollection<T>();
                        storage.AddRange(value);
                    }
                }
                else
                {
                    storage.ReplaceWhole(value);
                }
                OnPropertyChanged(propertyName);
                return true;
            }
        }
        
        [Obsolete("This method will soon be renamed")]
        protected void SubscribeTo<PropType>(CachedRelayProperty<PropType> relayProperty, string propertyName)
        {
            void relayProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(propertyName);
            }

            void relayProperty_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                OnPropertyChanging(propertyName);
            }

            //TODO: Fix Potential Memory Leaks
            relayProperty.PropertyChanging += relayProperty_PropertyChanging;
            relayProperty.PropertyChanged += relayProperty_PropertyChanged;
        }

        [Obsolete("This method will soon be renamed")]
        protected void SubscribeTo<PropType>(CachedReadOnlyProperty<PropType> relayProperty, string propertyName)
        {
            void relayProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(propertyName);
            }

            void relayProperty_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                OnPropertyChanging(propertyName);
            }

            //TODO: Fix Potential Memory Leaks
            relayProperty.PropertyChanging += relayProperty_PropertyChanging;
            relayProperty.PropertyChanged += relayProperty_PropertyChanged;
        }

        protected CachedReadOnlyProperty<T> CreateCachedReadOnlyProperty<T>(Func<T> updateFunction, string propertyName, INotifyPropertyChanged notifyObj = null, params string[] dependencies)
        {
            var result = CachedReadOnlyProperty.Create(updateFunction, notifyObj, dependencies);
            SubscribeTo(result, propertyName);
            return result;
        }

        protected CachedRelayProperty<T> CreateCachedRelayProperty<T>(Func<T> getFunc, Action<T> setAction, string propertyName,
                                                                      INotifyPropertyChanged notifyObj = null, params string[] dependecies)
        {
            var result = CachedRelayProperty.Create(getFunc, setAction, notifyObj, dependecies);
            SubscribeTo(result, propertyName);
            return result;
        }


        protected static string dotJoin(params string[] args)
        {
            return string.Join(".", args);
        }
    }
}
