using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public class CachedReadOnlyProperty<PropertyType> : NotifyBase
    {
        private Dictionary<INotifyPropertyChanged, List<string>> dependencies = new Dictionary<INotifyPropertyChanged, List<string>>();

        private bool isValuesSet = false;

        private PropertyType value = default;

        public PropertyType Value
        {
            get
            {
                if (!isValuesSet) { value = GetFunc(); isValuesSet = true; }
                return value;
            }
            protected set { Set(ref this.value, value); }
        }

        private Func<PropertyType> GetFunc;

        public CachedReadOnlyProperty(Func<PropertyType> getFunction, INotifyPropertyChanged notifyObj = null, IEnumerable<string> dependenceProperties = null)
        {
            this.GetFunc = getFunction;
            if (notifyObj != null && dependenceProperties != null)
            {
                AddDependencies(notifyObj, dependenceProperties);
            }
        }

        private void NotifyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        public CachedReadOnlyProperty<PropertyType> AddDependencies(INotifyPropertyChanged notifiable, params string[] propertyNames)
        {
            return AddDependencies(notifiable, (IEnumerable<string>)propertyNames);
        }

        public CachedReadOnlyProperty<PropertyType> AddDependencies(INotifyPropertyChanged notifiable, IEnumerable<string> propertyNames)
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
                notifiable.PropertyChanged += NotifyObject_PropertyChanged;
            }

            foreach (var propName in propertyNames)
            {
                if (!namesList.Contains(propName))
                    namesList.Add(propName);
            }
            return this;
        }

        public void DropValue()
        {
            isValuesSet = false;
            OnPropertyChanged(nameof(Value));
        }

        public void UpdateValue()
        {
            Value = GetFunc();
            isValuesSet = true;
        }

        ~CachedReadOnlyProperty()
        {
            foreach (var notifiable in dependencies.Keys)
            {
                notifiable.PropertyChanged -= NotifyObject_PropertyChanged;
            }
            dependencies.Clear();
        }

    }

    public static class CachedReadOnlyProperty
    {
        public static CachedReadOnlyProperty<PropType> Create<PropType>(Func<PropType> updateFunction, INotifyPropertyChanged notifyObj = null, IEnumerable<string> dependencies = null)
        {
            return new CachedReadOnlyProperty<PropType>(updateFunction, notifyObj, dependencies);
        }

        public static CachedReadOnlyProperty<PropType> Create<PropType>(Func<PropType> updateFunction, INotifyPropertyChanged notifyObj = null, params string[] dependencies)
        {
            return new CachedReadOnlyProperty<PropType>(updateFunction, notifyObj, dependencies);
        }
    }
}
