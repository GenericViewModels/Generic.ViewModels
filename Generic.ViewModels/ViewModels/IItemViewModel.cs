namespace GenericViewModels.ViewModels
{
    public interface IItemViewModel<out T>
        where T : class
    {
        T? Item { get; }
    }
}