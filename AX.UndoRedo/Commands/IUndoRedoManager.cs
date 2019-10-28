using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AX.UndoRedo
{
    public interface IUndoRedoManager : INotifyPropertyChanged
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        bool RecordingSessionInProgress { get; }
        int SessionDepth { get; }

        IReadOnlyObservableCollection<CommandRecord> UndoableCommands { get; }
        IReadOnlyObservableCollection<CommandRecord> RedoableCommands { get; }

        void Undo();
        void Redo();

        void Do(IUndoableCommand command, string commandName = "");
        void Record(IRecordableCommand command, string commandName = "");

        void StartCommandsRecordingSession(string transactionName = "");
        void AcceptSession();
        void RollbackSession();
    }
}
