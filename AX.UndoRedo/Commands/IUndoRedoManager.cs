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
        bool TransactionInProgress { get; }
        int TransactionDepth { get; }

        IReadOnlyObservableCollection<CommandRecord> UndoableCommands { get; }
        IReadOnlyObservableCollection<CommandRecord> RedoableCommands { get; }

        void Undo();
        void Redo();

        void Do(IUndoableCommand command, string commandName = "");
        void Record(IRecordableCommand command, string commandName = "");

        void StartTransaction(string transactionName = "");
        void CommitTransaction();
        void RefuseTransaction();
    }
}
