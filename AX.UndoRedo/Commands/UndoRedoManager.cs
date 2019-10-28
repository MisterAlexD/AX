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

            SubscribeProperty(nameof(RecordingSessionInProgress), nameof(SessionDepth));
        }

        public UndoRedoManager(int initialCapacity = 100)
        {
            undoCommands = new ObservableStack<CommandRecord>(new List<CommandRecord>(initialCapacity));
            redoCommands = new ObservableStack<CommandRecord>(new List<CommandRecord>(initialCapacity));
        }

        public bool CanUndo => !RecordingSessionInProgress && undoCommands.Count > 0;
        public bool CanRedo => !RecordingSessionInProgress && redoCommands.Count > 0;

        public bool RecordingSessionInProgress => transactionRecords.Count > 0;
        public int SessionDepth => transactionRecords.Count;

        public IReadOnlyObservableCollection<CommandRecord> UndoableCommands => undoCommands;
        public IReadOnlyObservableCollection<CommandRecord> RedoableCommands => redoCommands;

        public void Do(IUndoableCommand command, string commandName = "")
        {
            if (command != null)
            {
                if (RecordingSessionInProgress)
                {
                    command.Do();
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
                if (RecordingSessionInProgress)
                {
                    command.OnRecord();
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
            Debug.Assert(!RecordingSessionInProgress);
            if (RecordingSessionInProgress)
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
            Debug.Assert(!RecordingSessionInProgress);
            if (RecordingSessionInProgress)
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

        public void StartCommandsRecordingSession(string transactionName = "")
        {   
            transactionRecords.Push(new TransactionRecord { CommandsGroup = new CommandsGroup(), TransactionName = transactionName});
            OnPropertyChanged(nameof(SessionDepth));
        }

        public void AcceptSession()
        {
            Debug.Assert(RecordingSessionInProgress);
            if (RecordingSessionInProgress)
            {
                var transactionRecord = transactionRecords.Pop();
                if (transactionRecords.Count > 0)
                {
                    transactionRecords.Peek().CommandsGroup.Commands.Add(transactionRecord.CommandsGroup);
                }
                else
                {
                    redoCommands.Clear();
                    undoCommands.Push(new CommandRecord { Command = transactionRecord.CommandsGroup, CommandName = transactionRecord.TransactionName});
                    OnPropertyChanged(nameof(UndoableCommands));
                    OnPropertyChanged(nameof(RedoableCommands));
                }
                OnPropertyChanged(nameof(SessionDepth));
            }
        }

        public void RollbackSession()
        {
            Debug.Assert(RecordingSessionInProgress);
            if (RecordingSessionInProgress)
            {
                var transactionRecord = transactionRecords.Pop();

                transactionRecord.CommandsGroup.Undo();
               
                OnPropertyChanged(nameof(SessionDepth));
            }
        }

        private struct TransactionRecord
        {
            public CommandsGroup CommandsGroup { get; set; }
            public string TransactionName { get; set; }
        }
    }
}
