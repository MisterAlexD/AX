using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AX.UndoRedo
{
    public class ListInsertCommand<ItemsType> : IUndoableCommand
    {
        private readonly IList<ItemsType> list;
        private readonly ItemsType item;
        private readonly int index;

        public ListInsertCommand(IList<ItemsType> list, int index, ItemsType item)
        {
            Debug.Assert(list != null);
            Debug.Assert(index >= 0);

            this.list = list;
            this.index = index;
            this.item = item;
        }

        public void Do()
        {
            list.Insert(index, item);
        }

        public void Undo()
        {
            list.Remove(item);
        }
    }
}
