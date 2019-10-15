using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    public class ListAddCommand<ItemsType> : IUndoableCommand
    {
        private readonly IList<ItemsType> list;
        private readonly ItemsType newItem;

        public ListAddCommand(IList<ItemsType> list, ItemsType newItem)
        {
            this.list = list;
            this.newItem = newItem;
        }

        public void Do()
        {
            list.Add(newItem);
        }

        public void Undo()
        {
            list.Remove(newItem);
        }
    }
}
