using System.Threading.Tasks;

namespace GenericViewModels.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string page);
        Task GoBackAsync();
        string CurrentPage { get; }
    }
}
