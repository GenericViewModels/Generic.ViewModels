using GenericViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public abstract class ItemsService<T> : IItemsService<T>
    {
        protected AsyncEventSlim _initialized = new AsyncEventSlim();
        private readonly ISharedItems<T> _sharedItemsService;

        public event EventHandler<EventArgs> ItemsRefreshed
        {
            add => _sharedItemsService.ItemsRefreshed += value;
            remove => _sharedItemsService.ItemsRefreshed -= value;
        }

        protected void RaiseItemsRefreshed() => _sharedItemsService.RaiseItemsRefreshed();

        public ItemsService(ISharedItems<T> sharedItemsService)
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

        /// <summary>
        /// Items from the <see cref="ISharedItemsService{T}"/>
        /// </summary>
        public virtual ObservableCollection<T> Items => _sharedItemsService.Items;

        /// <summary>
        /// Override to add or update an item.
        /// </summary>
        /// <param name="item">The item to be added or updated</param>
        /// <returns>A <see cref="Task"/></returns>
        public virtual Task<T> AddOrUpdateAsync(T item) => Task.FromResult<T>(default);

        /// <summary>
        /// Override to delete the item.
        /// </summary>
        /// <param name="item">The item to delete</param>
        /// <returns>A <see cref="Task"/></returns>
        public virtual Task DeleteAsync(T item) => Task.FromResult<T>(default);

        /// <summary>
        /// Override to implement refreshing asyncs. Invoke this method to fire the ItemsRefreshed event.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        public virtual Task RefreshAsync()
        {
            RaiseItemsRefreshed();
            return Task.FromResult<T>(default);
        }
    }
}
