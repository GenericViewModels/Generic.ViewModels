using System;
using System.Collections.ObjectModel;

namespace GenericViewModels.Services
{
    public interface ISharedItems<T>
    {
        ObservableCollection<T> Items { get; }

        event EventHandler<EventArgs> ItemsRefreshed;
        void RaiseItemsRefreshed();
    }
}
