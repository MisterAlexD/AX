using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace AX.Common
{
    public interface IReadOnlyObservableCollection<out T> : IReadOnlyCollection<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
    }
}
