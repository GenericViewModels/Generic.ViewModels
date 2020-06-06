using GenericViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GenericViewModels.Services
{
    public class SharedItems<T> : BindableBase, ISharedItems<T>
        where T : class
    {
        public SharedItems()
        {
            Debug.WriteLine($"SharedItems ctor with {typeof(T).Name}");
        }

        private readonly ObservableCollection<T> _items = new ObservableCollection<T>();
        public virtual ObservableCollection<T> Items => _items;

        public event EventHandler<EventArgs>? ItemsRefreshed;
        public event EventHandler<SelectedItemEventArgs<T>>? SelectedItemChanged;

        public void RaiseItemsRefreshed() => ItemsRefreshed?.Invoke(this, new EventArgs());

        private T? _selectedItem;
        private readonly object _lockSelection = new object();
        public virtual T? SelectedItem
        {
            get
            {
                lock (_lockSelection)
                {
                    return _selectedItem;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if the selection was changed, false if it cannot be changed because of the edit mode, and null if no change is needed</returns>
        public virtual bool? SetSelectedItem(T item)
        {
            lock (_lockSelection)
            {
                if (SetProperty(ref _selectedItem, item, nameof(SelectedItem)))
                {
                    SelectedItemChanged?.Invoke(this, new SelectedItemEventArgs<T>(_selectedItem));
                    return true;
                }
                else
                {
                    return null;
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }
    }
}
