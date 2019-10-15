using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace AX
{
    public interface IReadOnlyObservableCollection<out T> : IReadOnlyCollection<T>, IObservableEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged
    { }
}
