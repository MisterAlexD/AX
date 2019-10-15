using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace AX
{
    public interface IObservableEnumerable<out T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged
    { }
}
