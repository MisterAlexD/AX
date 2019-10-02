using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    public interface ICachedReadOnlyProperty : INotifyPropertyChanged, INotifyPropertyChanging
    {
        List<string> Dependencies { get; }
        void UpdateValue();

        void AddDependencies(params string[] dependencies);
        void AddDependencies(IEnumerable<string> dependencies);
    }

    public interface ICachedReadOnlyProperty<T> : ICachedReadOnlyProperty
    {
        T Value { get; }
    }
}
