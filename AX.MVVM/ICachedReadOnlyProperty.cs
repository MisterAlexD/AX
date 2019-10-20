using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    [Obsolete("See no point using it", true)]
    public interface ICachedReadOnlyProperty : INotifyPropertyChanged, INotifyPropertyChanging
    {
        void UpdateValue();

        void AddDependencies(INotifyPropertyChanged notifiable, params string[] dependencies);
        void AddDependencies(INotifyPropertyChanged notifiable, IEnumerable<string> dependencies);
    }

    [Obsolete("See no point using it", true)]
    public interface ICachedReadOnlyProperty<T> 
    {
        T Value { get; }
    }
}
