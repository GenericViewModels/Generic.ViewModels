namespace Generic.ViewModels.Services
{
    public interface IItemToViewModelMap<T, TViewModel>
    {
        void Add(T item, TViewModel viewModel);
        TViewModel GetViewModel(T item);
        void Reset();
    }
}