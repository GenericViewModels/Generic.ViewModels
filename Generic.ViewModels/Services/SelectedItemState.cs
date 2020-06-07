using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GenericViewModels.Services
{
    [Obsolete("use SharedItem instead", error: true)]
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
