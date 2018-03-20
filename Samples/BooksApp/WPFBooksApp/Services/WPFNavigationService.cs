using GenericViewModels.Services;
using System;
using System.Threading.Tasks;

namespace WPFBooksApp.Services
{
    public class WPFNavigationService : INavigationService
    {
        public bool CanGoBack => throw new NotImplementedException();

        public string CurrentPage => throw new NotImplementedException();

        public Task GoBackAsync() => throw new NotImplementedException();
        public Task NavigateToAsync(string page) => throw new NotImplementedException();
    }
}
