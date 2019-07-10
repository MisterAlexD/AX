using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AX.WPF
{ 
    public class RenderEventArgs : EventArgs
    {
        public DrawingContext DrawingContext { get; private set; }

        public RenderEventArgs(DrawingContext dc)
        {
            this.DrawingContext = dc;
        }
    }
}
