using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class EditableItemViewModel<TItem> : ItemViewModel<TItem>, IEditableObject
        where TItem : class
    {
        private readonly IItemsService<TItem> _itemsService;
        private readonly ISharedItems<TItem> _sharedItems;
        private readonly ILogger<EditableItemViewModel<TItem>> _logger;

        public EditableItemViewModel(
            IItemsService<TItem> itemsService,
            ISharedItems<TItem> sharedItems,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(showProgressInfo)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _sharedItems = sharedItems ?? throw new ArgumentNullException(nameof(sharedItems));
            _logger = loggerFactory?.CreateLogger<EditableItemViewModel<TItem>>() ?? throw new ArgumentNullException(nameof(loggerFactory));

            _logger.LogTrace("ctor EditableItemViewModel");

            _sharedItems.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedItem")
                {
                    Item = _sharedItems.SelectedItem;
                    _logger.LogTrace($"PropertyChanged event from shareditems with SelectedItem received, changed Item to {Item}");
                }
            };

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Item))
                {
                    RaisePropertyChanged(nameof(EditItem));
                    _logger.LogTrace($"PropertyChanged event with Item received, firing change event on EditItem");
                }
            };

            EditCommand = new DelegateCommand(BeginEdit, () => IsReadMode);
            CancelCommand = new DelegateCommand(CancelEdit, () => IsEditMode);
            SaveCommand = new DelegateCommand(EndEdit, () => IsEditMode);
            AddCommand = new DelegateCommand(OnAdd, () => IsReadMode);
            DeleteCommand = new DelegateCommand(OnDelete);
        }


        public DelegateCommand AddCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }


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

            using (_showProgressInfo.StartInProgress(ProgressInfoName))
            {
                await OnDeleteCoreAsync();
                await _itemsService.RefreshAsync();
                SetSelectedItem(_sharedItems.Items.FirstOrDefault());

                await OnEndEditAsync();
            }
        }

        /// <summary>
        /// Sets the SelectedItem in SharedItems.SelectedItems
        /// </summary>
        protected virtual void SetSelectedItem(TItem item)
        {
            _logger.LogTrace($"SetSelectedItem - set selected and Item property to {item}");

            if (item == null) return;
            _sharedItems.SelectedItem = item;
            Item = item;
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
                    RaisePropertyChanged(nameof(IsReadMode));
                    CancelCommand.RaiseCanExecuteChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                    EditCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Copy Item for Edit Mode
        private TItem _editItem;

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
            _logger.LogTrace($"{nameof(BeginEdit)}, creating a copy of {Item}");

            if (Item == null) return;  // nothing selected

            IsEditMode = true;
            TItem itemCopy = CreateCopy(Item);
            if (itemCopy != null)
            {
                EditItem = itemCopy;
            }
        }

        /// <summary>
        /// set back to read mode
        /// intializes the EditItem property to return the Item property
        /// refreshes the item list
        /// </summary>
        public async virtual void CancelEdit()
        {
            _logger.LogTrace($"{nameof(CancelEdit)} with {EditItem}");

            IsEditMode = false;
            EditItem = default;

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
            _logger.LogTrace($"{nameof(EndEdit)} with {EditItem}");

            using (_showProgressInfo.StartInProgress(ProgressInfoName))
            {
                await OnSaveCoreAsync();
                int index = _sharedItems.Items.IndexOf(Item);  // with a new created item, its not in the Items collection
                if (index >= 0)
                {
                    _sharedItems.Items.RemoveAt(index);
                }
                Item = EditItem;
                if (index >= 0)
                {
                    _sharedItems.Items.Insert(index, Item);
                }
                else
                {
                    _sharedItems.Items.Add(Item);
                }
                EditItem = default;
                IsEditMode = false;

                SetSelectedItem(Item);

                await OnEndEditAsync();
            }
        }
        #endregion
    }
}
