using Generic.ViewModels.Services;
using GenericViewModels.Diagnostics;
using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// Derive a view-model from this class for master/detail functionality. 
    /// Items are wrapped in a view-model type for commands in a list (<see cref="Items"/>)
    /// <see cref="SelectedItem"/> returns the view-model type of the currently selected item
    /// 
    /// </summary>
    /// <typeparam name="TItemViewModel">The view-model type for the items</typeparam>
    /// <typeparam name="TItem">The item type</typeparam>
    public abstract class MasterDetailViewModel<TItemViewModel, TItem> : ViewModelBase
        where TItemViewModel : class, IItemViewModel<TItem>
        where TItem : class
    {
        protected IItemsService<TItem> ItemsService { get; }

        private readonly IItemToViewModelMap<TItem, TItemViewModel> _viewModelMap;

        public MasterDetailViewModel(
            IItemsService<TItem> itemsService,
            IItemToViewModelMap<TItem, TItemViewModel> viewModelMap,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(showProgressInfo, loggerFactory)
        {
            ItemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _viewModelMap = viewModelMap ?? throw new ArgumentNullException(nameof(viewModelMap));

            ItemsService.SelectedItemChanged += ItemsService_SelectedItemChanged;
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(SelectedItem))
                {
                    OnSelectedItemChanged(this, new SelectedItemEventArgs<TItemViewModel>(SelectedItem));
                }
            };

            ItemsService.Items.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(Items));
            };

            ItemsService.PropertyChanged += ItemsService_PropertyChanged;
        }

        /// <summary>
        /// Cleans up the associated event handlers
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ItemsService.SelectedItemChanged -= ItemsService_SelectedItemChanged;
                ItemsService.PropertyChanged -= ItemsService_PropertyChanged;
            }
        }

        private void ItemsService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEditMode")
            {
                // fire property change on IsReadMode which is used to hide the list in the UI
                RaisePropertyChanged(nameof(IsReadMode));
                RaisePropertyChanged(nameof(IsEditMode));
            }
        }

        public bool IsReadMode => !ItemsService.IsEditMode;
        public bool IsEditMode => ItemsService.IsEditMode;

        // forwards the property change event to SelectedItem
        private void ItemsService_SelectedItemChanged(object sender, SelectedItemEventArgs<TItem> e)
        {
            RaisePropertyChanged(nameof(SelectedItem));
        }

        /// <summary>
        /// Override to react to selected item change events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"><see cref="SelectedItemEventArgs{T}"/></param>
        protected virtual void OnSelectedItemChanged(object sender, SelectedItemEventArgs<TItemViewModel> e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            Logger.LogDebug(LoggingMessages.SelectedItemChanged(typeof(MasterDetailViewModel<TItemViewModel, TItem>), e.Item));
        }

        /// <summary>
        /// Invokes <see cref="RefreshAsync" on initialization/>
        /// Override for other intitialization.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        protected override Task InitCoreAsync() => RefreshAsync();

        /// <summary>
        /// Returns a view-model from the <see cref="ItemToViewModelMap{T, TViewModel}" cache/>
        /// </summary>
        /// <param name="item">the item to return the view-model</param>
        /// <returns><see cref="TItemViewModel"/></returns>
        protected virtual TItemViewModel? ToViewModel(TItem? item) => 
            _viewModelMap.GetViewModel(item);

        /// <summary>
        /// Returns items in the view-model representation.
        /// </summary>
        public virtual IEnumerable<TItemViewModel?> Items => 
            ItemsService.Items.Select(item => ToViewModel(item));

        /// <summary>
        /// Returns the view-model of the currently selected item.
        /// Use the set accessor to set the selected item using the associated <see cref="ItemsService{T}"/>
        /// </summary>
        public virtual TItemViewModel? SelectedItem
        {
            get => ToViewModel(ItemsService.SelectedItem);
            set
            {
                Logger.LogTrace($"SelectedItem updating to item {value?.Item}");
                if (value?.Item != null)
                {
                    ItemsService.SetSelectedItem(value.Item);
                }
            }
        }

        /// <summary>
        /// Call to load and refresh items 
        /// Override <see cref="OnRefreshCoreAsync" /> to implement 
        /// </summary>
        /// <returns></returns>
        protected async Task RefreshAsync()
        {
            Logger.LogTrace(LoggingMessages.Refresh(typeof(MasterDetailViewModel<TItemViewModel, TItem>)));

            using var progress = ShowProgressInfo.StartInProgress(ProgressInfoName);
            await OnRefreshCoreAsync();
        }

        /// <summary>
        /// Invokes RefreshAsync of the IItemsService service. Override for loading other data in your view-model.
        /// Invoked by <see cref="RefreshAsync"/>
        /// </summary>
        /// <returns><see cref="Task"/> to signal completion.</returns>
        protected virtual async Task OnRefreshCoreAsync()
        {
            await ItemsService.RefreshAsync();
        }

        /// <summary>
        /// Override to create an implementation to add a new item
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task OnAddCoreAsync() => Task.CompletedTask;
    }
}
