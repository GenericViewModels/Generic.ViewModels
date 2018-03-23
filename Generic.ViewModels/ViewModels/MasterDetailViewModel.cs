using GenericViewModels.Services;
using Prism.Commands;
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

        public MasterDetailViewModel(IItemsService<TItem> itemsService)
        {
            _itemsService = itemsService;

            _itemsService.Items.CollectionChanged += (sender, e) =>
            {
                base.RaisePropertyChanged(nameof(ItemsViewModels));
            };

            RefreshCommand = new DelegateCommand(OnRefresh);
            AddCommand = new DelegateCommand(OnAdd);
        }

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
                if (!EqualityComparer<TItem>.Default.Equals(_itemsService.SelectedItem, value))
                {
                    _itemsService.SelectedItem = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(SelectedItemViewModel));
                }
            }
        }

        public virtual TItemViewModel SelectedItemViewModel
        {
            get => ToViewModel(_itemsService.SelectedItem);
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
        /// </summary>
        public async void OnRefresh()
        {
            using (StartInProgress())
            {
                await OnRefreshCoreAsync();
                SelectedItem = _itemsService.Items.FirstOrDefault();
            }
        }

        /// <summary>
        /// invokes RefreshAsync of the IItemsService service
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
