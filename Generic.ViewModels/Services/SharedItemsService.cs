using System.Collections.ObjectModel;

namespace GenericViewModels.Services
{
    public class SharedItemsService<T> : ISharedItemsService<T>
    {
        private readonly ObservableCollection<T> _items = new ObservableCollection<T>();
        public virtual ObservableCollection<T> Items => _items;
    }
}
