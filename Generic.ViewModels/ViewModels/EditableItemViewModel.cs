using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class EditableItemViewModel<TItem> : ItemViewModel<TItem>, IEditableObject, IDisposable
        where TItem : class
    {
        protected IItemsService<TItem> ItemsService { get; }
        protected ILogger Logger { get; }

        public EditableItemViewModel(
            IItemsService<TItem> itemsService,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(showProgressInfo)
        {
            ItemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            Logger = loggerFactory?.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));

            Logger.LogTrace("ctor EditableItemViewModel");

            ItemsService.SelectedItemChanged += OnSelectedItemChanged;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Item))
                {
                    Logger.LogTrace($"PropertyChanged event with Item received, firing change event on EditItem");
                    RaisePropertyChanged(nameof(EditItem));
                }
            };
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
                ItemsService.SelectedItemChanged -= OnSelectedItemChanged;
            }
        }

        protected virtual void OnSelectedItemChanged(object sender, SelectedItemEventArgs<TItem> e)
        {
            Logger.LogTrace($"SelectedItemChanged event from items service received, setting Item to {e.Item}");
            Item = e.Item;
        }

        /// <summary>
        /// Overriding this method is required to start the OnDelete method
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task<bool> AreYouSureAsync() => Task.FromResult(false);

        /// <summary>
        /// Override this method for an implementation of the delete functionality, e.g. to call a service
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected abstract Task OnDeleteCoreAsync();
        protected async void OnDelete()
        {
            if (!await AreYouSureAsync())
            {
                return;
            }

            using var progress = ShowProgressInfo.StartInProgress(ProgressInfoName);
            await OnDeleteCoreAsync();
            await ItemsService.RefreshAsync();
            SetSelectedItem(ItemsService.Items.FirstOrDefault());

            await OnEndEditAsync();
        }

        /// <summary>
        /// Sets the SelectedItem in SharedItems.SelectedItems which raises the event SelecteItemsChanged
        /// </summary>
        protected virtual bool? SetSelectedItem(TItem item)
        {
            Logger.LogTrace($"SetSelectedItem - set selected and Item property to {item}");

            if (item == null) return null;
            bool? result = ItemsService.SetSelectedItem(item);
            if (result == true)
            {
                Item = item;
            }
            return result;
        }

        #region Edit / Read Mode
        private bool _isEditMode;
        /// <summary>
        /// Returns true if the item is in read mode
        /// </summary>
        public bool IsReadMode => !IsEditMode;

        /// <summary>
        /// Returns true if the item is in edit mode
        /// </summary>
        public virtual bool IsEditMode
        {
            get => _isEditMode;
            protected set
            {
                if (SetProperty(ref _isEditMode, value))
                {
                    ItemsService.IsEditMode = _isEditMode;

                    RaisePropertyChanged(nameof(IsReadMode));

                    OnEditCommandChanges();
                }
            }
        }

        protected abstract void OnEditCommandChanges();

        #endregion

        #region Copy Item for Edit Mode
        private TItem? _editItem;

        /// <summary>
        /// EditItem returns the Item in read mode
        /// and contains a copy of the Item in edit mode
        /// </summary>
        public TItem EditItem
        {
            get => _editItem ?? Item;
            set => SetProperty(ref _editItem, value);
        }

        #endregion

        /// <summary>
        /// Implement to create a copy of the item to be used by the IEditableObject implementation
        /// </summary>
        /// <param name="item">The item to copy</param>
        /// <returns>The copied item</returns>
        protected abstract TItem CreateCopy(TItem item);

        #region Overrides Needed By Derived Class
        /// <summary>
        /// Override for an implementation to save the EditItem, e.g. to invoke a service to store the item
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected abstract Task OnSaveCoreAsync();

        /// <summary>
        /// Override for an implementation to be invoked from the EndEdit method (implementation of IEditableObject).
        /// This method is invoked at the end of the EndEdit method before progress completion
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task OnEndEditAsync() => Task.CompletedTask;

        /// <summary>
        /// Invoked from the AddCommand. Invokes <see cref="OnAddCoreAsync"/>. Override this method for an implementation" />
        /// </summary>
        protected async void OnAdd()
        {
            await OnAddCoreAsync();
        }

        /// <summary>
        /// Create an implementation to ...
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        protected virtual Task OnAddCoreAsync() => Task.CompletedTask;

        #endregion

        #region IEditableObject

        /// <summary>
        /// preparations for edit mode
        /// sets IsEditMode
        /// creates a copy of the item and sets the EditItem property
        /// </summary>
        public virtual void BeginEdit()
        {
            Logger.LogTrace($"{nameof(BeginEdit)}, creating a copy of {Item}");

            if (Item == null) return;  // nothing selected

            IsEditMode = true;
            TItem itemCopy = CreateCopy(Item);
            if (itemCopy != null)
            {
                EditItem = itemCopy;
            }
        }

        private void ResetEditItem()
        {
            _editItem = default;
            RaisePropertyChanged(nameof(EditItem));
        }

        /// <summary>
        /// set back to read mode
        /// intializes the EditItem property to return the Item property
        /// refreshes the item list
        /// </summary>
        public async virtual void CancelEdit()
        {
            Logger.LogTrace($"{nameof(CancelEdit)} with {EditItem}");

            IsEditMode = false;
            ResetEditItem();

            await OnEndEditAsync();
        }

        /// <summary>
        /// setup progress information
        /// invoke OnSaveCoreAsync 
        /// resets EditItem
        /// invokes RefreshAsync of the IItemsService, and sets the Item from the selected item
        /// </summary>
        public async virtual void EndEdit()
        {
            Logger.LogTrace($"{nameof(EndEdit)} with {EditItem}");

            using var progress = ShowProgressInfo.StartInProgress(ProgressInfoName);
            await OnSaveCoreAsync();
            int index = ItemsService.Items.IndexOf(Item);  // with a new created item, its not in the Items collection
            if (index >= 0)
            {
                ItemsService.Items.RemoveAt(index);
            }
            Item = EditItem;
            if (index >= 0)
            {
                ItemsService.Items.Insert(index, Item);
            }
            else
            {
                ItemsService.Items.Add(Item);
            }
            ResetEditItem();
            IsEditMode = false;

            SetSelectedItem(Item);

            await OnEndEditAsync();
        }
        #endregion
    }
}
