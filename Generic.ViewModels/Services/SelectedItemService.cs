using System;

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
                _selectedItem = value;
                SelectedItemChanged?.Invoke(this, _selectedItem);
            }
        }

        public event EventHandler<T> SelectedItemChanged;
    }
}
