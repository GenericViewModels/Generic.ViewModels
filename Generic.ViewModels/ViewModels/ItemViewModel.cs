using GenericViewModels.Services;

namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// base class for view-models that shows a single item
    /// </summary>
    /// <typeparam name="T">Item type for the view-model to display</typeparam>
    public abstract class ItemViewModel<T> : ViewModelBase, IItemViewModel<T>
        where T : class
    {
        public ItemViewModel(IShowProgressInfo showProgressInfo)
            : base(showProgressInfo)
        {
        }

        public ItemViewModel(T item, IShowProgressInfo showProgressInfo) 
            : base(showProgressInfo)
            => _item = item;

        private T _item;
        public virtual T Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }
    }
}
