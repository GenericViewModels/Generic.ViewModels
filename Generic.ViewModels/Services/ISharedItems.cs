using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GenericViewModels.Services
{
    public interface ISharedItems<T> : INotifyPropertyChanged
    {
        ObservableCollection<T> Items { get; }

        event EventHandler<EventArgs> ItemsRefreshed;
        void RaiseItemsRefreshed();

        T SelectedItem { get; set; }
    }
}
