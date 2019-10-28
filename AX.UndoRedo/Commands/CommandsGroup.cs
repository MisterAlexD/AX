using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace AX.UndoRedo
{
    public class CommandsGroup : IUndoableCommand
    {
        public string GroupName { get; set; }
        public List<IUndoableCommand> Commands { get; private set; } = new List<IUndoableCommand>(5);

        public CommandsGroup()
        { }

        public CommandsGroup(IEnumerable<IUndoableCommand> commands)
        {
            Commands.AddRange(commands);
        }

        public CommandsGroup(params IUndoableCommand[] commands)
        {
            Commands.AddRange(commands);
        }

        public void Do()
        {
            Commands.ForEach(c => c.Do());
        }

        public void Undo()
        {
            Commands.Reverse<IUndoableCommand>().ForEach(c => c.Undo());
        }
    }
}
