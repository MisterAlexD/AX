using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    public partial class ViewModelBase<T> : NotifyBase, IViewModel<T>
        where T : INotifyPropertyChanged
    {

    }
}
