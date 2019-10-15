using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    public class RecordableCommandsGroup : IRecordableCommand
    {
        public string GroupName { get; set; }
        public List<IRecordableCommand> Commands { get; private set; } = new List<IRecordableCommand>(5);

        public RecordableCommandsGroup()
        { }

        public RecordableCommandsGroup(IEnumerable<IRecordableCommand> commands)
        {
            Commands.AddRange(commands);
        }

        public RecordableCommandsGroup(params IRecordableCommand[] commands)
        {
            Commands.AddRange(commands);
        }

        public void Do()
        {
            Commands.ForEach(c => c.Do());
        }

        public void Undo()
        {
            Commands.ForEach(c => c.Undo());
        }

        public void OnRecord()
        {
            Commands.ForEach(c => c.OnRecord());
        }
    }
}
