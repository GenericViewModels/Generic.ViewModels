using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string page);
        bool CanGoBack { get; }
        Task GoBackAsync();
        string CurrentPage { get; }
    }
}
