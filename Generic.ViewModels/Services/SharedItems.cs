using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GenericViewModels.Services
{
    public class SharedItems<T> : BindableBase, ISharedItems<T>
    {
        public SharedItems()
        {
            Debug.WriteLine($"SharedItems ctor with {typeof(T).Name}");
        }

        private readonly ObservableCollection<T> _items = new ObservableCollection<T>();
        public virtual ObservableCollection<T> Items => _items;

        public event EventHandler<EventArgs> ItemsRefreshed;

        public void RaiseItemsRefreshed() => ItemsRefreshed?.Invoke(this, new EventArgs());

        private T _selectedItem;
        private object _lockSelection = new object();
        public virtual T SelectedItem
        {
            get
            {
                lock (_lockSelection)
                {
                    return _selectedItem;
                }
            }

            set
            {
                lock (_lockSelection)
                {
                    SetProperty(ref _selectedItem, value);
                }
            }
        }
    }
}
