using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    public interface IReadOnlyObservableEnumerable<out T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }
}
