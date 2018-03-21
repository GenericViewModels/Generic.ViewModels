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

        public EditableItemViewModel(IItemsService<TItem> itemsService)
        {
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            Item = _itemsService.SelectedItem;

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

        protected abstract Task OnDeleteAsync();
        protected virtual Task<bool> AreYouSureAsync() => Task.FromResult(false);
        private async void OnDelete()
        {
            if (!await AreYouSureAsync()) return;

            using (StartInProgress())
            {
                await OnDeleteAsync();
                await _itemsService.RefreshAsync();
                Item = _itemsService.SelectedItem;
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
        /// I
        /// </summary>
        /// <returns></returns>
        protected abstract Task OnSaveAsync();
        protected virtual Task OnEndEditAsync() => Task.CompletedTask;
        protected abstract void OnAdd();

        #endregion

        #region IEditableObject

        public virtual void BeginEdit()
        {
            IsEditMode = true;
            TItem itemCopy = CreateCopy(Item);
            if (itemCopy != null)
            {
                EditItem = itemCopy;
            }
        }

        public async virtual void CancelEdit()
        {
            IsEditMode = false;
            EditItem = default(TItem);
            await _itemsService.RefreshAsync();
            await OnEndEditAsync();
        }

        public async virtual void EndEdit()
        {
            using (StartInProgress())
            {
                await OnSaveAsync();
                EditItem = default(TItem);
                IsEditMode = false;
                await _itemsService.RefreshAsync();
                Item = _itemsService.SelectedItem;
                await OnEndEditAsync();
            }
        }
        #endregion
    }
}
