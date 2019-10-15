using System;
using System.Collections.Generic;
using System.Text;

namespace AX.UndoRedo
{
    public class ListRemoveCommand<ItemsType> : IUndoableCommand
    {
        private readonly IList<ItemsType> list;
        private readonly ItemsType item;
        private int index = -1;

        public ListRemoveCommand(IList<ItemsType> list, ItemsType item)
        {
            this.list = list;
            this.item = item;
        }

        public void Do()
        {
            if (index == -1)
            {
                index = list.IndexOf(item);
            }
            list.RemoveAt(index);
        }

        public void Undo()
        {
            list.Insert(index, item);
        }
    }

    public static class ListRemoveCommand
    {
        public static ListRemoveCommand<T> Create<T>(IList<T> list, T item) => new ListRemoveCommand<T>(list, item);
    }
}
