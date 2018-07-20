using System;
using System.Collections.Generic;

namespace GenericViewModels.Services
{
    public class SelectedItemService<T> : ISelectedItemService<T>
    {
        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_selectedItem, value))
                {
                    _selectedItem = value;
                    SelectedItemChanged?.Invoke(this, _selectedItem);
                }
            }
        }

        public event EventHandler<T> SelectedItemChanged;
    }
}
