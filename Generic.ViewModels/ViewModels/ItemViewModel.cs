namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// base class for view-models that shows a single item
    /// </summary>
    /// <typeparam name="T">Item type for the view-model to display</typeparam>
    public abstract class ItemViewModel<T> : ViewModelBase, IItemViewModel<T>
    {
        private T _item;
        public virtual T Item
        {
            get => _item;
            set => Set(ref _item, value);
        }
    }
}
