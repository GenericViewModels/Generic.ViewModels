using System;
using System.Collections.Generic;

namespace GenericViewModels.Services
{
    public class SelectedItemState<T> : ISelectedItem<T>
    {
        private T _selectedItem;
        public virtual T SelectedItem
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
