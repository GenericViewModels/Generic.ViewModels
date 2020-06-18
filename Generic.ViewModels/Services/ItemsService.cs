using GenericViewModels.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public abstract class ItemsService<T> : BindableBase, IItemsService<T>, IDisposable
        where T : class
    {
        protected AsyncEventSlim _initialized = new AsyncEventSlim();
        private readonly ISharedItems<T> _sharedItems;
        private readonly ILogger _logger;

        public event EventHandler<EventArgs> ItemsRefreshed
        {
            add => _sharedItems.ItemsRefreshed += value;
            remove => _sharedItems.ItemsRefreshed -= value;
        }
        public event EventHandler<SelectedItemEventArgs<T>> SelectedItemChanged
        {
            add => _sharedItems.SelectedItemChanged += value;
            remove => _sharedItems.SelectedItemChanged -= value;
        }

#pragma warning disable CA1030 // Use events where appropriate
        protected void RaiseItemsRefreshed() => _sharedItems.RaiseItemsRefreshed();
#pragma warning restore CA1030 // Use events where appropriate

        public ItemsService(ISharedItems<T> sharedItemsService, ILoggerFactory loggerFactory)
        {
            _sharedItems = sharedItemsService ?? throw new ArgumentNullException(nameof(sharedItemsService));
            _logger = loggerFactory?.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));

            _sharedItems.PropertyChanged += SharedItems_PropertyChanged;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sharedItems.PropertyChanged -= SharedItems_PropertyChanged;
            }
        }

        private void SharedItems_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem" || e.PropertyName == "IsEditMode")
            {
                _logger.LogTrace($"PropertyChanged event from shared items received - property {e.PropertyName} - firing event");

                RaisePropertyChanged(e.PropertyName);
            }

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
        public virtual ObservableCollection<T> Items => _sharedItems.Items;

        public virtual T? SelectedItem
        {
            get => _sharedItems.SelectedItem;
        }

        public virtual bool IsEditMode
        {
            get => _sharedItems.IsEditMode;
            set => _sharedItems.IsEditMode = value;
        }

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
        public virtual Task DeleteAsync(T item) => Task.CompletedTask;

        /// <summary>
        /// Override to implement refreshing asyncs. Invoke this method to fire the ItemsRefreshed event.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        public virtual Task RefreshAsync()
        {
            _logger.LogTrace("RefreshAsync - firing ItemsRefreshed event");
            RaiseItemsRefreshed();
            return Task.CompletedTask;
        }

        public bool? SetSelectedItem(T item) => _sharedItems.SetSelectedItem(item);
    }
}
