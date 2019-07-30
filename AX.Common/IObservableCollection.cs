using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AX.Common
{
    public interface IObservableCollection<T> : ICollection<T>, INotifyCollectionChanged
    {
    }
}
