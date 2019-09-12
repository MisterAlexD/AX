using AX.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AX.MVVM
{   
    
    public class VMReadOnlyCollection<ViewModelType, ModelType> : IReadOnlyList<ViewModelType>, INotifyCollectionChanged
        where ViewModelType : IViewModel<ModelType>
    {
        private IEnumerable<ModelType> modelsCollection;

        private SubscribableCollection<ViewModelType> internalList = new SubscribableCollection<ViewModelType>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ViewModelType this[int index] => internalList[index];

        public int Count => internalList.Count;

        private readonly Func<ModelType, ViewModelType> VMFabric;

        public VMReadOnlyCollection(IEnumerable<ModelType> modelsCollection, Func<ModelType, ViewModelType> VMFabric)
        {
            this.modelsCollection = modelsCollection;
            this.VMFabric = VMFabric;

            foreach (var item in modelsCollection)
            {
                internalList.Add(VMFabric(item));
            }

            if (modelsCollection is INotifyCollectionChanged notifiable)
            {
                notifiable.CollectionChanged += ModelsCollection_CollectionChanged;
            }

            this.internalList.CollectionChanged += InternalList_CollectionChanged;
        }

        private void InternalList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void ModelsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                internalList.Clear();
                foreach (var model in modelsCollection)
                {
                    internalList.Add(VMFabric(model));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex > -1)
                {
                    foreach (ModelType model in e.NewItems.Cast<ModelType>().Reverse())
                    {
                        internalList.Insert(e.NewStartingIndex, VMFabric(model));
                    }
                }
                else
                {
                    internalList.AddRange(e.NewItems.Cast<ModelType>().Select(model => VMFabric(model)));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                internalList.Move(e.OldStartingIndex, e.NewStartingIndex);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedArray = e.OldItems.Cast<ModelType>().ToArray();
                internalList.RemoveAll(x => removedArray.Contains(x.Model));
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                internalList[e.OldStartingIndex] = VMFabric((ModelType)e.NewItems[0]);
            }
        }

        public IEnumerator<ViewModelType> GetEnumerator() => internalList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => internalList.GetEnumerator();
    }
}
