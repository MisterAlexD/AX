using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    public interface ICachedReadOnlyProperty : INotifyPropertyChanged, INotifyPropertyChanging
    {
        string PropertyName { get; }
    }
}
