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
        /// invokes <see cref="RefreshAsync"/> and sets the <see cref="SelectedItem"/> property
        /// Invoked by the <see cref="RefreshCommand"/> 
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
        /// Invokes RefreshAsync of the IItemsService service. Override for more refresh needs.
        /// </summary>
        /// <returns>> <see cref="Task"/></returns>
        protected virtual async Task OnRefreshCoreAsync() =>
            await _itemsService.RefreshAsync();

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
