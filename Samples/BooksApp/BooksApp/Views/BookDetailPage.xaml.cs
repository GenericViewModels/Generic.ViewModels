using BooksLib.ViewModels;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BooksApp.Views
{
    public sealed partial class BookDetailPage : Page
    {
        public BookDetailPage()
        {
            this.InitializeComponent();
            ViewModel.UseNavigation = true; // if the Page is used, enable navigation

            var navigationService = (Application.Current as App).AppServices.GetService<INavigationService>();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        public BookDetailViewModel ViewModel { get; } = (Application.Current as App).AppServices.GetService<BookDetailViewModel>();
    }
}
