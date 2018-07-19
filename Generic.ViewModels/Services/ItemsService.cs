using GenericViewModels.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public abstract class ItemsService<T> : IItemsService<T>
    {
        private AsyncEventSlim _initalized = new AsyncEventSlim();

        public ItemsService()
        {
            Task t = InitAsync();
        }

        private async Task InitAsync()
        {
            try
            {
                await InitAsyncCore();
            }
            finally
            {
                _initalized.Signal();
            }
        }

        protected Task InitAsyncCore() => Task.CompletedTask;

        public abstract ObservableCollection<T> Items { get; }

        public abstract Task<T> AddOrUpdateAsync(T item);
        public abstract Task DeleteAsync(T item);
        public abstract Task RefreshAsync();
    }
}
