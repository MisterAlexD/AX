using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace AX.MVVM
{
    public class CachedRelayProperty<T> : NotifyBase, ICachedReadOnlyProperty
    {
        private Func<T> GetFunc;
        private Action<T> SetAction;

        INotifyPropertyChanged notifyObj;
        public List<string> Dependencies { get; private set; }

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

            if (notifyObj != null)
            {
                this.notifyObj = notifyObj;
                notifyObj.PropertyChanged += NotifyObj_PropertyChanged;
            }
            Dependencies = new List<string>(5);
            if (Dependencies != null)
            {
                Dependencies.AddRange(dependeceProperties);
            }

        }

        private void NotifyObj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.IsAny(Dependencies))
            {
                UpdateValue();
            }
        }

        public void DropValue()
        {
            isValueSet = false;
            OnPropertyChanged(nameof(Value));
        }

        public void UpdateValue()
        {
            value = GetFunc();
            OnPropertyChanged(nameof(Value));
        }

        public void AddDependencies(params string[] dependencies)
        {
            Dependencies.AddRange(dependencies);
        }

        public void AddDependencies(IEnumerable<string> dependencies)
        {
            Dependencies.AddRange(dependencies);
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
