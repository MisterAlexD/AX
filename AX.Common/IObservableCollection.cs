using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AX
{    
    public interface IObservableCollection<T> : ICollection<T>, INotifyCollectionChanged
    { }
}
