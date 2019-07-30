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
        protected sealed class VMService<ModelType, ViewModelType>
            where ViewModelType : NotifyBase, IViewModel<ModelType>
                where ModelType : INotifyPropertyChanged
        {
            public IVMProperty<PropType> CreateVMProperty<PropType>(string propName, ViewModelType vm, Func<ModelType, PropType> getFunc, Action<ModelType, PropType> setAction, params string[] dependencies)
            {
                return new VMProperty<ModelType, ViewModelType, PropType>(propName, vm, getFunc, setAction, dependencies);
            }
        }
    }
}
