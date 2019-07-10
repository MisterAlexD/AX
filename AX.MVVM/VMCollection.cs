using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using AX.Common;

namespace AX.MVVM
{
    public class VMCollection<ViewModelType, ModelType> :  SubscribableCollection<ViewModelType>, IReadOnlyList<ViewModelType>, INotifyCollectionChanged
        where ViewModelType : IViewModel<ModelType>
    {
        private IList<ModelType> modelsCollection;
        private readonly Func<ModelType, ViewModelType> VMFabric;

        public bool IsReadOnly => throw new NotImplementedException();

        public VMCollection(IList<ModelType> modelsCollection, Func<ModelType, ViewModelType> VMFabric)
        {
            this.modelsCollection = modelsCollection;
            this.VMFabric = VMFabric;

            foreach (var item in modelsCollection)
            {
                this.Add(VMFabric(item));
            }

            if (modelsCollection is INotifyCollectionChanged notifiable)
            {
                notifiable.CollectionChanged += ModelsCollection_CollectionChanged;
            }

            this.CollectionChanged += This_CollectionChanged;
        }

        bool suppressNotification = false;

        private void This_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!suppressNotification)
            {
                suppressNotification = true;

                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    //Will never happen because we are derived from SubscribableCollection
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewStartingIndex > -1)
                    {
                        e.NewItems.Cast<ViewModelType>().Reverse().ForEach(vm => modelsCollection.Insert(e.NewStartingIndex, vm.Model));
                    }
                    else
                    {
                        e.NewItems.Cast<ViewModelType>().ForEach(vm => modelsCollection.Add(vm.Model));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    var removedItem = modelsCollection[e.OldStartingIndex];
                    modelsCollection.RemoveAt(e.OldStartingIndex);
                    modelsCollection.Insert(e.NewStartingIndex, removedItem);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var modelsToRemove = e.OldItems.Cast<ViewModelType>().Select(vm => vm.Model).ToList();
                    modelsToRemove.ForEach(m => modelsCollection.Remove(m));
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    modelsCollection.RemoveAt(e.OldStartingIndex);
                    modelsCollection.Insert(e.NewStartingIndex, ((ViewModelType)e.NewItems[0]).Model);
                }
                

                suppressNotification = false;
            }
        }

        private void ModelsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!suppressNotification)
            {
                suppressNotification = true;
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    this.Clear();
                    foreach(var model in modelsCollection)
                    {
                        this.Add(VMFabric(model));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    if (e.NewStartingIndex > -1)
                    {
                        foreach (ModelType model in e.NewItems.Cast<ModelType>().Reverse())
                        {
                            this.Insert(e.NewStartingIndex, VMFabric(model));
                        }
                    }
                    else
                    {
                        this.AddRange(e.NewItems.Cast<ModelType>().Select(model => VMFabric(model)));
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    this.Move(e.OldStartingIndex, e.NewStartingIndex);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var removePositions = e.OldItems.Cast<ModelType>().Select(m => modelsCollection.IndexOf(m));
                    var viewModelsToRemove = removePositions.Select(index => this[index]).ToList();
                    this.RemoveRange(viewModelsToRemove);
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    this[e.OldStartingIndex] = VMFabric((ModelType)e.NewItems[0]);
                }

                suppressNotification = false;
            }
        }

        
    }
}
