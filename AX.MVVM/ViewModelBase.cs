using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public partial class ViewModelBase<T> : NotifyBase, IViewModel<T>
        where T : INotifyPropertyChanged
    {
        private T model;

        public T Model
        {
            get { return model; }
            internal set { SetAndSubscribe(ref model, value); }
        }
    
        public ViewModelBase(T model)
        {
            Model = model;
        }

        protected static string modelPrefix(string propertyName)
        {
            return "Model." + propertyName;
        }
    }
}
