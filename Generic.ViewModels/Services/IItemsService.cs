using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public interface IItemsService<T>
    {
        event EventHandler<EventArgs> ItemsRefreshed;

        Task RefreshAsync();

        Task<T> AddOrUpdateAsync(T item);

        Task DeleteAsync(T item);

        ObservableCollection<T> Items { get; }
    }
}
