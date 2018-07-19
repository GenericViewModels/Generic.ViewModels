using System.Collections.ObjectModel;

namespace GenericViewModels.Services
{
    public interface ISharedItemsService<T>
    {
        ObservableCollection<T> Items { get; }
    }
}
