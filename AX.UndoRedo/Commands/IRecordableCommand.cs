using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    /// <summary>
    /// Представляет команду действия которой уже были выполнены
    /// </summary>
    public interface IRecordableCommand : IUndoableCommand
    {
        void OnRecord();
    }
}
