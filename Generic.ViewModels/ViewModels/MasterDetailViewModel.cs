using Generic.ViewModels.Services;
using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class MasterDetailViewModel<TItemViewModel, TItem> : ViewModelBase, IDisposable
        where TItemViewModel : IItemViewModel<TItem>
        where TItem : class
    {
        protected readonly IItemsService<TItem> _itemsService;
        protected readonly ILogger _logger;
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

            RefreshCommand = new DelegateCommand(OnRefresh);
            AddCommand = new DelegateCommand(OnAdd);
        }

        public virtual void Dispose()
        {
            _itemsService.SelectedItemChanged -= ItemsService_SelectedItemChanged;
        }

        private void ItemsService_SelectedItemChanged(object sender, SelectedItemEventArgs<TItem> e)
        {
            _logger.LogTrace($"SelectedItem change event received fom items service with {e.Item}");
            RaisePropertyChanged(nameof(SelectedItem));
        }

        protected override Task InitCoreAsync() => RefreshAsync();

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand AddCommand { get; }


        protected virtual TItemViewModel ToViewModel(TItem item) => _viewModelMap.GetViewModel(item);

        public virtual IEnumerable<TItemViewModel> Items => _itemsService.Items.Select(item => ToViewModel(item));

        public virtual TItemViewModel SelectedItem
        {
            get => ToViewModel(_itemsService.SelectedItem);
            set
            {
                _logger.LogTrace($"SelectedItem updating to item {value?.Item}");
                _itemsService.SelectedItem = value?.Item;
            }
        }

        /// <summary>
        /// preparations for progress information,
        /// invokes <see cref="RefreshAsync"/> and sets the <see cref="SelectedItem"/> property
        /// Invoked by the <see cref="RefreshCommand"/> 
        /// </summary>
        protected async void OnRefresh() => await RefreshAsync();

        private async Task RefreshAsync()
        {
            _logger.LogTrace($"{nameof(RefreshAsync)}");

            using (_showProgressInfo.StartInProgress(ProgressInfoName))
            {
                await OnRefreshCoreAsync();
                _logger.LogTrace($"{nameof(RefreshAsync)}");

            }
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
        /// Override <see cref="OnAddCoreAsync" to prepare adding an item/>
        /// </summary>
        protected async void OnAdd() => await OnAddCoreAsync();

        /// <summary>
        /// Override to create an implementation to add a new item
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task OnAddCoreAsync() => Task.CompletedTask;
    }
}
