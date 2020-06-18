using Generic.ViewModels.Services;
using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class MasterDetailViewModel<TItemViewModel, TItem> : ViewModelBase, IDisposable
        where TItemViewModel : class, IItemViewModel<TItem>
        where TItem : class
    {
        private readonly IItemsService<TItem> _itemsService;
        private readonly ILogger _logger;
        protected IItemsService<TItem> ItemsService => _itemsService;
        protected ILogger Logger => _logger;

        private readonly IItemToViewModelMap<TItem, TItemViewModel> _viewModelMap;

        public MasterDetailViewModel(
            IItemsService<TItem> itemsService,
            IItemToViewModelMap<TItem, TItemViewModel> viewModelMap,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(showProgressInfo)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _viewModelMap = viewModelMap ?? throw new ArgumentNullException(nameof(viewModelMap));
            _logger = loggerFactory?.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger.LogTrace("ctor MasterDetailViewModel");

            _itemsService.SelectedItemChanged += ItemsService_SelectedItemChanged;

            _itemsService.Items.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(Items));
            };

            _itemsService.PropertyChanged += ItemsService_PropertyChanged;
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
                _itemsService.SelectedItemChanged -= ItemsService_SelectedItemChanged;
                _itemsService.PropertyChanged -= ItemsService_PropertyChanged;
            }
        }

        private void ItemsService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEditMode")
            {
                // fire property change on IsReadMode which is used to hide the list in the UI
                RaisePropertyChanged(nameof(IsReadMode));
            }
        }

        public bool IsReadMode => !ItemsService.IsEditMode;

        private void ItemsService_SelectedItemChanged(object sender, SelectedItemEventArgs<TItem> e)
        {
            _logger.LogTrace($"SelectedItem change event received fom items service with {e.Item}");
            RaisePropertyChanged(nameof(SelectedItem));
        }

        protected override Task InitCoreAsync() => RefreshAsync();

        protected virtual TItemViewModel? ToViewModel(TItem item) => _viewModelMap.GetViewModel(item);

        public virtual IEnumerable<TItemViewModel> Items => ItemsService.Items.Select(item => ToViewModel(item));

        public virtual TItemViewModel? SelectedItem
        {
            get => ToViewModel(_itemsService.SelectedItem);
            set
            {
                _logger.LogTrace($"SelectedItem updating to item {value?.Item}");
                if (value?.Item != null)
                {
                    _itemsService.SetSelectedItem(value.Item);
                }
            }
        }

        protected async Task RefreshAsync()
        {
            _logger.LogTrace($"{nameof(RefreshAsync)}");

            using var progress = ShowProgressInfo.StartInProgress(ProgressInfoName);
            await OnRefreshCoreAsync();
            _logger.LogTrace($"{nameof(RefreshAsync)}");
        }

        /// <summary>
        /// Invokes RefreshAsync of the IItemsService service. Override for more refresh needs.
        /// </summary>
        /// <returns>> <see cref="Task"/></returns>
        protected virtual async Task OnRefreshCoreAsync()
        {
            _logger.LogTrace($"{nameof(OnRefreshCoreAsync)}");

            await _itemsService.RefreshAsync();
        }

        /// <summary>
        /// Override to create an implementation to add a new item
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task OnAddCoreAsync() => Task.CompletedTask;
    }
}
