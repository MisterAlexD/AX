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
        public interface IVMReadOnlyProperty<PropType>
        {
            PropType Value { get; }
        }

        public class VMReadOnlyProperty<ModelType, ViewModelType, PropType> : NotifyBase, IVMReadOnlyProperty<PropType>
            where ViewModelType : NotifyBase, IViewModel<ModelType>
            where ModelType : INotifyPropertyChanged
        {
            private Func<ModelType, PropType> Getter;

            protected ViewModelType viewModel;

            private string propName;

            bool shouldUpdate = true;
            private PropType value;
            public PropType Value
            {
                get
                {
                    if (shouldUpdate)
                    {
                        var newValue = Getter(viewModel.Model);
                        if (!EqualityComparer<PropType>.Default.Equals(newValue, value))
                        {
                            value = newValue;
                            OnPropertyChanged();
                        }
                        shouldUpdate = false;
                    }
                    return value;
                }
            }

            private List<string> dependencies;

            public VMReadOnlyProperty(string propName, ViewModelType vm, Func<ModelType, PropType> getFunc, params string[] dependencies)
            {
                this.dependencies = new List<string>();

                this.propName = propName;
                viewModel = vm;
                viewModel.PropertyChanging += ViewModel_PropertyChanging;
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
                Getter = getFunc;
                SubscribeTo(dependencies);
            }

            public void SubscribeTo(params string[] dependencies)
            {
                //this.dependecies.AddRange(dependecies.Select(x => "Model." + x));
                this.dependencies.AddRange(dependencies);
            }

            public void SubscribeTo(IEnumerable<string> dependencies)
            {
                //this.dependecies.AddRange(dependecies.Select(x => "Model." + x));
                this.dependencies.AddRange(dependencies);
            }

            private void ViewModel_PropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                if (e.IsAny(dependencies))
                {
                    viewModel.CallPropertyChanging(propName);
                }
            }

            private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.IsAny(dependencies))
                {
                    shouldUpdate = true;
                    viewModel.CallPropertyChanged(propName);
                }
            }
        }

        public interface IVMProperty<PropType> : IVMReadOnlyProperty<PropType>
        {
            new PropType Value { get; set; }
            void SubscribeTo(params string[] dependencies);
        }

        public class VMProperty<ModelType, ViewModelType, PropType> : VMReadOnlyProperty<ModelType, ViewModelType, PropType>, IVMProperty<PropType>
            where ViewModelType : NotifyBase, IViewModel<ModelType>
            where ModelType : INotifyPropertyChanged
        {
            private Action<ModelType, PropType> Setter;

            public new PropType Value
            {
                get { return base.Value; }
                set
                {
                    Setter(viewModel.Model, value);
                }
            }

            public VMProperty(string propName, ViewModelType vm, Func<ModelType, PropType> getFunc, Action<ModelType, PropType> setAction, params string[] dependencies)
                : base(propName, vm, getFunc, dependencies)
            {
                Setter = setAction;
            }
        }

        public class VMReadOnlyProperty<PropType> : VMReadOnlyProperty<T, ViewModelBase<T>, PropType>
        {
            public VMReadOnlyProperty(string propName, ViewModelBase<T> vm, Func<T, PropType> getFunc, params string[] dependencies) :
                base(propName, vm, getFunc, dependencies)
            {
            }
        }

        public class VMProperty<PropType> : VMProperty<T, ViewModelBase<T>, PropType>
        {
            public VMProperty(string propName, ViewModelBase<T> vm, Func<T, PropType> getFunc, Action<T, PropType> setAction, params string[] dependencies)
                : base(propName, vm, getFunc, setAction, dependencies)
            {
            }
        }

    }
    


}
