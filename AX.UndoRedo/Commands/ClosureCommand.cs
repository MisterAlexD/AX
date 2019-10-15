using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    [Obsolete("Closure command is experimental feature. It iss highly advised to provide your own type of command. Use at own risk. ", false)]
    public class ClosureCommand : IUndoableCommand
    {
        private readonly Action doAction;
        private readonly Action undoAction;

        public ClosureCommand(Action doAction, Action undoAction)
        {
            this.doAction = doAction;
            this.undoAction = undoAction;
        }

        public void Do()
        {
            doAction();
        }

        public void Undo()
        {
            undoAction();
        }
    }
}
