using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    public interface IUndoableCommand
    {
        void Do();
        void Undo();
    }
}
