using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace AX.MVVM
{
    public class CachedRelayProperty<T> : NotifyBase
    {
        private Func<T> GetFunc;
        private Action<T> SetAction;

        private Dictionary<INotifyPropertyChanged, List<string>> dependencies = new Dictionary<INotifyPropertyChanged, List<string>>();

        private bool isValueSet = false;

        private T value;

        public T Value
        {
            get
            {
                if (!isValueSet) { value = GetFunc(); isValueSet = true; }
                return value;
            }
            set { SetAction(value); }
        }

        public CachedRelayProperty(Func<T> getFunction, Action<T> setAction, INotifyPropertyChanged notifyObj = null, IEnumerable<string> dependeceProperties = null)
        {
            Debug.Assert(getFunction != null);
            Debug.Assert(setAction != null);
            GetFunc = getFunction;
            SetAction = setAction;
            if (notifyObj != null && dependeceProperties != null)
            {
                SubscribeTo(notifyObj, dependeceProperties);
            }

        }

        private void NotifyObj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var notifyable = sender as INotifyPropertyChanged;
            if (dependencies.ContainsKey(notifyable))
            {
                if (e.IsAny(dependencies[notifyable]))
                {
                    DropValue();
                }
            }
        }

        public void DropValue()
        {
            isValueSet = false;
            OnPropertyChanged(nameof(Value));
        }

        public void UpdateValue()
        {
            Value = GetFunc();
            isValueSet = true;
        }

        public CachedRelayProperty<T> SubscribeTo(INotifyPropertyChanged notifiable, params string[] propertyNames)
        {
            return SubscribeTo(notifiable, (IEnumerable<string>)propertyNames);
        }

        public CachedRelayProperty<T> SubscribeTo(INotifyPropertyChanged notifiable, IEnumerable<string> propertyNames)
        {
            List<string> namesList = null;
            if (dependencies.ContainsKey(notifiable))
            {
                namesList = dependencies[notifiable];
            }
            else
            {
                namesList = new List<string>(5);
                dependencies.Add(notifiable, namesList);
                notifiable.PropertyChanged += NotifyObj_PropertyChanged;
            }

            foreach (var propName in propertyNames)
            {
                if (!namesList.Contains(propName))
                    namesList.Add(propName);
            }
            return this;
        }

        ~CachedRelayProperty()
        {
            foreach (var notifiable in dependencies.Keys)
            {
                notifiable.PropertyChanged -= NotifyObj_PropertyChanged;
            }
            dependencies.Clear();
        }
    }

    public static class CachedRelayProperty
    {
        public static CachedRelayProperty<T> Create<T>(Func<T> getFunc, Action<T> setAction)
        {
            return new CachedRelayProperty<T>(getFunc, setAction);
        }

        public static CachedRelayProperty<T> Create<T>(Func<T> getFunc, Action<T> setAction, INotifyPropertyChanged notifyObj, params string[] dependecies)
        {
            return new CachedRelayProperty<T>(getFunc, setAction, notifyObj, dependecies);
        }

        public static CachedRelayProperty<T> Create<T>(Func<T> getFunc, Action<T> setAction, INotifyPropertyChanged notifyObj, IEnumerable<string> dependecies)
        {
            return new CachedRelayProperty<T>(getFunc, setAction, notifyObj, dependecies);
        }
    }
}
