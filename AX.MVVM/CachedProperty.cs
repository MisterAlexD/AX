using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public class CachedProperty<PropertyType, LinkedObjectType>
        where LinkedObjectType : NotifyBase
    {
        private List<string> dependenceNames = new List<string>();

        private PropertyType value = default(PropertyType);
        private bool needToUpdate = true;

        private LinkedObjectType linkedObject = default(LinkedObjectType);

        public string PropertyName { get; private set; }

        public PropertyType Value
        {
            get
            {
                if (needToUpdate)
                {
                    value = UpdateFunction(linkedObject);
                    needToUpdate = false;
                }
                return value;
            }
        }

        private Func<LinkedObjectType, PropertyType> UpdateFunction;

        public CachedProperty(string propertyName, Func<LinkedObjectType, PropertyType> updateFunction, LinkedObjectType linkedObject)
        {
            this.PropertyName = propertyName;
            this.UpdateFunction = updateFunction;
            this.linkedObject = linkedObject;
            this.linkedObject.PropertyChanged += LinkedObject_PropertyChanged;
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
            needToUpdate = true;
            linkedObject.CallPropertyChanged(PropertyName);
        }

    }

    public class CachedProperty<PropertyType> : CachedProperty<PropertyType, NotifyBase>
    {
        public CachedProperty(string propertyName, Func<NotifyBase, PropertyType> updateFunction, NotifyBase linkedObject) :
            base(propertyName, updateFunction, linkedObject)
        { }
    }
}
