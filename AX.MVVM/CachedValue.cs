using System;
using System.Collections.Generic;
using System.Text;

namespace AX.MVVM
{       
    public class CachedValue<ValueType, LinkedObjectType>
    {
        private ValueType value = default(ValueType);
        private bool needToUpdate = true;
        private readonly LinkedObjectType linkedObject;
        private readonly Func<LinkedObjectType, ValueType> updateFunc;

        public ValueType Value
        {
            get
            {
                if (needToUpdate)
                {
                    value = updateFunc(linkedObject);
                }
                return value;
            }
        }

        public CachedValue(Func<LinkedObjectType, ValueType> updateFunc, LinkedObjectType linkedObject = default(LinkedObjectType))
        {
            this.updateFunc = updateFunc;
            this.linkedObject = linkedObject;
        }

        void UpdateValue()
        {
            needToUpdate = true;
        }
    }

    public class CachedValue<ValueType> : CachedValue<ValueType, object>
    {
        public CachedValue(Func<object, ValueType> updateFunc, object linkedObject = null)
            : base(updateFunc, linkedObject)
        {
        }
    }
}
