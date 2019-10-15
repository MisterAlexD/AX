using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.UndoRedo
{
    public class RecordableChangePropertyCommand : ChangePropertyCommand, IRecordableCommand
    {
        public RecordableChangePropertyCommand(object obj, string propertyName, object oldValue, object newValue)
            :base(obj, propertyName, newValue)
        {
            this.oldValue = oldValue;
        }

        public RecordableChangePropertyCommand(object obj, PropertyDescriptor propertyDescriptor, object oldValue, object newValue)
            :base(obj, propertyDescriptor, newValue)
        {
            this.oldValue = oldValue;
        }

      

        public void OnRecord()
        {
            //Do nothing
        }
    }
}
