using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    public struct CommandRecord
    {
        internal IUndoableCommand Command { get; set; }
        public string CommandName { get; set; }
    }
}
