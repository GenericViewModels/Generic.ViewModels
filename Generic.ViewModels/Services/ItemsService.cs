using GenericViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public abstract class ItemsService<T> : IItemsService<T>
    {
        protected AsyncEventSlim _initialized = new AsyncEventSlim();
        private readonly ISharedItemsService<T> _sharedItemsService;

        public ItemsService(ISharedItemsService<T> sharedItemsService)
        {
            _sharedItemsService = sharedItemsService ?? throw new ArgumentNullException(nameof(sharedItemsService));
        }

        /// <summary>
        /// Call this method from the constructor of the base class if an asynchronous intialization is needed.
        /// Invokes the overrideable method InitCoreAsync, and signals completion with the _initialized event
        /// </summary>
        /// <returns>A <see cref="Task" that is completed when the event is signalled/></returns>
        protected async Task InitAsync()
        {
            try
            {
                await InitCoreAsync();
            }
            finally
            {
                _initialized.Signal();
            }
        }

        /// <summary>
        /// Override this method in case of async completion is needed
        /// </summary>
        /// <returns>A <see cref="Task" that is completed when the event is signalled/></returns>
        protected virtual Task InitCoreAsync() => Task.CompletedTask;

        public virtual ObservableCollection<T> Items => _sharedItemsService.Items;

        public virtual Task<T> AddOrUpdateAsync(T item) => Task.FromResult<T>(default);
        public virtual Task DeleteAsync(T item) => Task.FromResult<T>(default);
        public virtual Task RefreshAsync() => Task.FromResult<T>(default);
    }
}
