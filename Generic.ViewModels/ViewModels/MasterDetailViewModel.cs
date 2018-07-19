using GenericViewModels.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class MasterDetailViewModel<TItemViewModel, TItem> : ViewModelBase
        where TItemViewModel : IItemViewModel<TItem>
        where TItem : class
    {
        private readonly IItemsService<TItem> _itemsService;
        private readonly ISelectedItemService<TItem> _selectedItemService;

        public MasterDetailViewModel(IItemsService<TItem> itemsService, ISelectedItemService<TItem> selectedItemService)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _selectedItemService = selectedItemService ?? throw new ArgumentNullException(nameof(selectedItemService));

            _itemsService.Items.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(ItemsViewModels));
            };

            RefreshCommand = new DelegateCommand(OnRefresh);
            AddCommand = new DelegateCommand(OnAdd);
        }

        /// <summary>
        /// Invokes RefreshAsync
        /// </summary>
        /// <returns>a task</returns>
        protected override Task InitCoreAsync() => RefreshAsync();
  
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand AddCommand { get; }

        public ObservableCollection<TItem> Items => _itemsService.Items;

        protected abstract TItemViewModel ToViewModel(TItem item);

        public virtual IEnumerable<TItemViewModel> ItemsViewModels => Items.Select(item => ToViewModel(item));

        public virtual TItem SelectedItem
        {
            get => _selectedItemService.SelectedItem;
            set
            {
                if (!EqualityComparer<TItem>.Default.Equals(_selectedItemService.SelectedItem, value))
                {
                    _selectedItemService.SelectedItem = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(SelectedItemViewModel));
                }
            }
        }

        public virtual TItemViewModel SelectedItemViewModel
        {
            get => ToViewModel(_selectedItemService.SelectedItem);
            set
            {
                if (value != null && !EqualityComparer<TItem>.Default.Equals(SelectedItem, value.Item))
                {
                    SelectedItem = value.Item;
                }
            }
        }

        /// <summary>
        /// preparations for progress information,
        /// invokes OnRefreshCoreAsync and sets the SelectedItem property
        /// Invoked by the RefreshCommand
        /// </summary>
        protected async void OnRefresh() => await RefreshAsync();

        private async Task RefreshAsync()
        {
            using (StartInProgress())
            {
                await OnRefreshCoreAsync();
                SelectedItem = _itemsService.Items.FirstOrDefault();
            }
        }

        /// <summary>
        /// Invokes RefreshAsync of the IItemsService service
        /// </summary>
        /// <returns>a task</returns>
        protected virtual async Task OnRefreshCoreAsync() =>
            await _itemsService.RefreshAsync();

        protected async void OnAdd() => await OnAddCoreAsync();

        /// <summary>
        /// override it to create an implementation to add a new item
        /// </summary>
        /// <returns>a task</returns>
        protected virtual Task OnAddCoreAsync() => Task.CompletedTask;
    }
}
