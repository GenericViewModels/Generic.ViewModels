using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GenericViewModels.Services
{
    public interface ISharedItems<T> : INotifyPropertyChanged
        where T : class
    {
        ObservableCollection<T> Items { get; }

        event EventHandler<EventArgs> ItemsRefreshed;
        event EventHandler<SelectedItemEventArgs<T>> SelectedItemChanged;

        void RaiseItemsRefreshed();

        T? SelectedItem { get;  }
        bool? SetSelectedItem(T item);
        
        bool IsEditMode { get; set; }
    }
}
