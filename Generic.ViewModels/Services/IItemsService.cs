﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public class SelectedItemEventArgs<T> : EventArgs
    {
        public SelectedItemEventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; }
    }

    public interface IItemsService<T>
    {
        event EventHandler<EventArgs> ItemsRefreshed;
        event EventHandler<SelectedItemEventArgs<T>> SelectedItemChanged;

        Task RefreshAsync();

        Task<T> AddOrUpdateAsync(T item);

        Task DeleteAsync(T item);

        ObservableCollection<T> Items { get; }

        T SelectedItem { get; set; }
    }
}
