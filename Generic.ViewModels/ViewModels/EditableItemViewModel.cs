using GenericViewModels.Services;
using Prism.Commands;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    public abstract class EditableItemViewModel<TItem> : ItemViewModel<TItem>, IEditableObject
        where TItem : class
    {
        private readonly IItemsService<TItem> _itemsService;
        private readonly ISelectedItemService<TItem> _selectedItemService;

        public EditableItemViewModel(IItemsService<TItem> itemsService, ISelectedItemService<TItem> selectedItemService)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _selectedItemService = selectedItemService ?? throw new ArgumentNullException(nameof(selectedItemService));

            Item = _selectedItemService.SelectedItem;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Item))
                {
                    RaisePropertyChanged(nameof(EditItem));
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

        protected abstract Task OnDeleteCoreAsync();
        protected virtual Task<bool> AreYouSureAsync() => Task.FromResult(false);
        private async void OnDelete()
        {
            if (!await AreYouSureAsync()) return;

            using (StartInProgress())
            {
                await OnDeleteCoreAsync();
                await _itemsService.RefreshAsync();
                Item = _selectedItemService.SelectedItem;
                await OnEndEditAsync();
            }
        }

        #region Edit / Read Mode
        private bool _isEditMode;
        public bool IsReadMode => !IsEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            private set
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

        protected abstract TItem CreateCopy(TItem item);

        #region Overrides Needed By Derived Class
        /// <summary>
        /// override for an implementation to save the EditItem
        /// </summary>
        /// <returns>a task</returns>
        protected abstract Task OnSaveCoreAsync();
        protected virtual Task OnEndEditAsync() => Task.CompletedTask;
        protected async void OnAdd() => await OnAddCoreAsync();

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
            IsEditMode = false;
            EditItem = default(TItem);
            await _itemsService.RefreshAsync();
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
            using (StartInProgress())
            {
                await OnSaveCoreAsync();
                EditItem = default(TItem);
                IsEditMode = false;
                await _itemsService.RefreshAsync();
                Item = _selectedItemService.SelectedItem;
                await OnEndEditAsync();
            }
        }
        #endregion
    }
}
