using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace AX.MVVM
{
    public partial class ViewModelBase<T> : NotifyBase, IViewModel<T>
        where T : INotifyPropertyChanged
    {

    }

}
