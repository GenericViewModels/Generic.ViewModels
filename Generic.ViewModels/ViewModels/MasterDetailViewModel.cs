using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class MasterDetailViewModel<TItemViewModel, TItem> : ViewModelBase, IDisposable
        where TItemViewModel : IItemViewModel<TItem>
        where TItem : class
    {
        protected readonly IItemsService<TItem> _itemsService;
        private readonly ILogger<MasterDetailViewModel<TItemViewModel, TItem>> _logger;

        public MasterDetailViewModel(
            IItemsService<TItem> itemsService,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(showProgressInfo)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _logger = loggerFactory?.CreateLogger<MasterDetailViewModel<TItemViewModel, TItem>>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger.LogTrace("ctor MasterDetailViewModel");

            _itemsService.SelectedItemChanged += ItemsService_SelectedItemChanged;

            _itemsService.Items.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(ItemsViewModels));
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
            RaisePropertyChanged(nameof(SelectedItemViewModel));
        }

        protected override Task InitCoreAsync() => RefreshAsync();

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand AddCommand { get; }

        public ObservableCollection<TItem> Items => _itemsService.Items;

        protected abstract TItemViewModel ToViewModel(TItem item);

        public virtual IEnumerable<TItemViewModel> ItemsViewModels => Items.Select(item => ToViewModel(item));

        public virtual TItem SelectedItem
        {
            get => _itemsService.SelectedItem;
            set
            {
                _logger.LogTrace($"{nameof(SelectedItem)} updating to {value}");
                _itemsService.SelectedItem = value;
            }
        }

        public virtual TItemViewModel SelectedItemViewModel
        {
            get => ToViewModel(_itemsService.SelectedItem);
            set
            {
                if (value != null && !EqualityComparer<TItem>.Default.Equals(SelectedItem, value.Item))
                {
                    _logger.LogTrace($"SelectedItemViewModel updating to item {value?.Item}");
                    SelectedItem = value.Item;
                }
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
