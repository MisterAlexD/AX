using AX.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AX.UndoRedo
{
    public class UndoRedoManager : NotifyBase, IUndoRedoManager
    {
        private ObservableStack<CommandRecord> undoCommands;
        private ObservableStack<CommandRecord> redoCommands;

        private Stack<TransactionRecord> transactionRecords = new Stack<TransactionRecord>(10);

        static UndoRedoManager()
        {
            SubscribeProperty(nameof(CanUndo), nameof(UndoableCommands));
            SubscribeProperty(nameof(CanRedo), nameof(RedoableCommands));

            SubscribeProperty(nameof(TransactionInProgress), nameof(TransactionDepth));
        }

        public UndoRedoManager(int initialCapacity = 100)
        {
            undoCommands = new ObservableStack<CommandRecord>(new List<CommandRecord>(initialCapacity));
            redoCommands = new ObservableStack<CommandRecord>(new List<CommandRecord>(initialCapacity));
        }

        public bool CanUndo => !TransactionInProgress && undoCommands.Count > 0;
        public bool CanRedo => !TransactionInProgress && redoCommands.Count > 0;

        public bool TransactionInProgress => transactionRecords.Count > 0;
        public int TransactionDepth => transactionRecords.Count;

        public IReadOnlyObservableCollection<CommandRecord> UndoableCommands => undoCommands;
        public IReadOnlyObservableCollection<CommandRecord> RedoableCommands => redoCommands;

        public void Do(IUndoableCommand command, string commandName = "")
        {
            if (command != null)
            {
                if (TransactionInProgress)
                {
                    transactionRecords.Peek().CommandsGroup.Commands.Add(command);
                }
                else
                {
                    command.Do();
                    redoCommands.Clear();
                    undoCommands.Push(new CommandRecord { Command = command, CommandName = commandName });
                    OnPropertyChanged(nameof(UndoableCommands));
                    OnPropertyChanged(nameof(RedoableCommands));
                }
            }
        }

        public void Record(IRecordableCommand command, string commandName = "")
        {
            if (command != null)
            {
                if (TransactionInProgress)
                {
                    transactionRecords.Peek().CommandsGroup.Commands.Add(command);
                }
                else
                {
                    command.OnRecord();
                    redoCommands.Clear();
                    undoCommands.Push(new CommandRecord { Command = command, CommandName = commandName });
                    OnPropertyChanged(nameof(UndoableCommands));
                    OnPropertyChanged(nameof(RedoableCommands));
                }
            }
        }

        public void Undo()
        {
            Debug.Assert(!TransactionInProgress);
            if (TransactionInProgress)
            {
                throw new InvalidOperationException("Can't undo while transaction in progress");
            }

            Debug.Assert(CanUndo);
            if (CanUndo)
            {
                var commandRecord = undoCommands.Pop();
                commandRecord.Command.Undo();
                redoCommands.Push(commandRecord);
                OnPropertyChanged(nameof(UndoableCommands));
                OnPropertyChanged(nameof(RedoableCommands));
            }
        }

        public void Redo()
        {
            Debug.Assert(!TransactionInProgress);
            if (TransactionInProgress)
            {
                throw new InvalidOperationException("Can't redo while transaction in progress.");
            }
            Debug.Assert(CanRedo);
            if (CanRedo)
            {
                var commandRecord = redoCommands.Pop();
                commandRecord.Command.Do();
                redoCommands.Push(commandRecord);
                OnPropertyChanged(nameof(UndoableCommands));
                OnPropertyChanged(nameof(RedoableCommands));
            }
        }

        public void StartTransaction(string transactionName = "")
        {   
            transactionRecords.Push(new TransactionRecord { CommandsGroup = new CommandsGroup(), TransactionName = transactionName});
            OnPropertyChanged(nameof(TransactionDepth));
        }

        public void CommitTransaction()
        {
            Debug.Assert(TransactionInProgress);
            if (TransactionInProgress)
            {
                var transactionRecord = transactionRecords.Pop();
                Do(transactionRecord.CommandsGroup);
                OnPropertyChanged(nameof(TransactionDepth));
            }
        }

        public void RefuseTransaction()
        {
            Debug.Assert(TransactionInProgress);
            if (TransactionInProgress)
            {
                var transactionRecord = transactionRecords.Pop();
                //Возвращаем значения для записываемых комманд, так как их действия уже были выполнены
                transactionRecord.CommandsGroup.Commands.OfType<IRecordableCommand>().ForEach(command => command.Undo());
               
                OnPropertyChanged(nameof(TransactionDepth));
            }
        }

        private struct TransactionRecord
        {
            public CommandsGroup CommandsGroup { get; set; }
            public string TransactionName { get; set; }
        }
    }
}
