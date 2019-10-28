using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AX.WPF
{
    public interface IFilter
    {
        bool UseFilter(object item);
    }
}
