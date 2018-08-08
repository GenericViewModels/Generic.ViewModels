using BooksLib.ViewModels;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace BooksApp.Views
{
    public sealed partial class BookDetailPage : Page
    {
        private IServiceScope _scope;

        public BookDetailPage()
        {
            this.InitializeComponent();
            _scope = AppServices.Instance.ServiceProvider.CreateScope();

            this.Unloaded += (sender, e) => _scope.Dispose();

            ViewModel.UseNavigation = true; // if the Page is used, enable navigation

            var navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();

            ViewModel = _scope.ServiceProvider.GetRequiredService<BookDetailViewModel>();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public BookDetailViewModel ViewModel { get; }
    }
}
