using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public interface INavigationService
    {
        bool UseNavigation { get; set; }
        Task NavigateToAsync(string page);
        bool CanGoBack { get; }
        Task GoBackAsync();
        string CurrentPage { get; }
    }
}
