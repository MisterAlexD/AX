using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AX.UndoRedo
{
    /// <summary>
    /// Только для использования внутри UndoRedo библиотеки
    /// Функциональность минимально урезана чтобы убрать предложения Intellisense
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ObservableStack<T> : ObservableCollection<T>, IReadOnlyObservableCollection<T>
    {
        public ObservableStack()
            :base()
        {

        }

        public ObservableStack(List<T> list)
            :base(list)
        {

        }

        public ObservableStack(IEnumerable<T> collection)
           : base(collection)
        {

        }

        private new void Add(T item) { }
        private new void Remove(T item) { }
        private new void Move(int oldIndex, int newIndex) { }
        private new void Insert(int index, T item) { }

        protected override void ClearItems()
        {
            var items = this.Items.ToArray();
            items.ForEach(x => base.Remove(x));
        }

        public T Peek()
        {
            return Items[Count - 1];
        }

        public T Pop()
        {
            var last = Peek();
            base.Remove(last);
            return last;
        }

        public void Push(T item)
        {
            base.Add(item);
        }
    }
}
