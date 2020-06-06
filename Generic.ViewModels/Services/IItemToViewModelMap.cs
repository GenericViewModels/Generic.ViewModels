namespace Generic.ViewModels.Services
{
    public interface IItemToViewModelMap<T, TViewModel>
        where T : class
        where TViewModel : class
    {
        void Add(T item, TViewModel viewModel);
        TViewModel? GetViewModel(T item);
        void Reset();
    }
}