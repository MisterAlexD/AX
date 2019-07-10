using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.MVVM
{
    public interface IViewModel<out T>
    {
        T Model { get; }
    }
}
