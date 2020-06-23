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

#pragma warning disable CA1030 // Use events where appropriate - used to fire events from outside
        void RaiseItemsRefreshed();
#pragma warning restore CA1030 // Use events where appropriate

        T? SelectedItem { get;  }
        bool? SetSelectedItem(T item);
        
        bool IsEditMode { get; set; }
    }
}
