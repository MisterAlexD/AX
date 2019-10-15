using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace AX.UndoRedo
{
    public class ChangePropertyCommand : IUndoableCommand
    {
        protected readonly PropertyDescriptor propertyDescriptor;
        protected readonly object obj;
        protected object oldValue = null; //Специально не указан readonly, так как мы хотим захватить старое значение именно в момент первого выполнения
        protected readonly object newValue;

        public ChangePropertyCommand(object obj, string propertyName, object newValue)
        {
            this.obj = obj;
            this.newValue = newValue;

            Debug.Assert(obj != null);
            Debug.Assert(propertyName != null);

            propertyDescriptor = TypeDescriptor.GetProperties(obj.GetType()).Find(propertyName, false);

            Debug.Assert(propertyDescriptor != null);
        }

        public ChangePropertyCommand(object obj, PropertyDescriptor propertyDescriptor, object newValue)
        {
            Debug.Assert(obj != null);
            Debug.Assert(propertyDescriptor != null);

            this.obj = obj;
            this.propertyDescriptor = propertyDescriptor;
            this.newValue = newValue;
        }

        public void Do()
        {
            if (oldValue == null)
            {
                oldValue = propertyDescriptor.GetValue(obj);
            }
            propertyDescriptor.SetValue(obj, newValue);
        }

        public void Undo()
        {
            propertyDescriptor.SetValue(obj, oldValue);
        }
    }
}
