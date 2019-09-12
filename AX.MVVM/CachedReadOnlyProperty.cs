using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public class CachedReadOnlyProperty<PropertyType, LinkedObjectType> : NotifyBase, ICachedReadOnlyProperty
    {
        private List<string> dependenceNames = new List<string>();

        private LinkedObjectType linkedObject = default(LinkedObjectType);

        public string PropertyName { get; private set; }

        private PropertyType value = default(PropertyType);

        public PropertyType Value
        {
            get { return value; }
            protected set { Set(ref value, value); }
        }

        private Func<LinkedObjectType, PropertyType> UpdateFunction;

        public CachedReadOnlyProperty(string propertyName, LinkedObjectType linkedObject, Func<LinkedObjectType, PropertyType> updateFunction)
        {
            this.PropertyName = propertyName;
            this.UpdateFunction = updateFunction;
            this.linkedObject = linkedObject;
            if (linkedObject is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += LinkedObject_PropertyChanged;
            }
            if (linkedObject is NotifyBase notifyBase)
            {
                notifyBase.SubscribeCachedReadOnlyProperty(this);
            }
        }

        private void LinkedObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.IsAny(dependenceNames))
            {
                UpdateValue();
            }
        }

        public void SubsribeTo(params string[] propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (!dependenceNames.Contains(propName))
                    dependenceNames.Add(propName);
            }
        }

        public void SubsribeTo(IEnumerable<string> propertyNames)
        {
            foreach (var propName in propertyNames)
            {
                if (!dependenceNames.Contains(propName))
                    dependenceNames.Add(propName);
            }
        }

        public void UpdateValue()
        {
            var oldValue = value;
            var newValue = UpdateFunction(linkedObject);
            Value = newValue;
        }
    }
    
    public static class CachedReadOnlyProperty
    {
        public static CachedReadOnlyProperty<PropType, ObjType> Create<PropType, ObjType>(string propertyName, ObjType linkedObject, Func<ObjType, PropType> updateFunction)
        {
            return new CachedReadOnlyProperty<PropType, ObjType>(propertyName, linkedObject, updateFunction);
        }
    }


}
