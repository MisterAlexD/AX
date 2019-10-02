using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public class CachedReadOnlyProperty<PropertyType> : NotifyBase, ICachedReadOnlyProperty<PropertyType>
    {
        public List<string> Dependencies { get; private set; }

        private PropertyType value = default;

        public PropertyType Value
        {
            get { return value; }
            protected set { Set(ref value, value); }
        }

        private Func<PropertyType> GetFunc;

        public CachedReadOnlyProperty(Func<PropertyType> getFunction, INotifyPropertyChanged notifyObj = null, IEnumerable<string> dependenceProperties = null)
        {
            Dependencies = new List<string>(5);
                
            this.GetFunc = getFunction;
            if (notifyObj != null)
            {
                notifyObj.PropertyChanged += NotifyObject_PropertyChanged;
            }
        }

        private void NotifyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.IsAny(Dependencies))
            {
                UpdateValue();
            }
        }

        public void AddDependencies(params string[] propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (!Dependencies.Contains(propName))
                    Dependencies.Add(propName);
            }
        }

        public void AddDependencies(IEnumerable<string> propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (!Dependencies.Contains(propName))
                    Dependencies.Add(propName);
            }
        }

        public void UpdateValue()
        {
            Value = GetFunc();
        }
    }
    
    public static class CachedReadOnlyProperty
    {
        public static CachedReadOnlyProperty<PropType> Create<PropType>(Func<PropType> updateFunction, INotifyPropertyChanged notifyObj = null, IEnumerable<string> dependencies = null)
        {
            return new CachedReadOnlyProperty<PropType>(updateFunction, notifyObj, dependencies);
        }
    }


}
