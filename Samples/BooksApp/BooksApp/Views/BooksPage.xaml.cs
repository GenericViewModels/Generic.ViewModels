using BooksLib.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace BooksApp.Views
{
    public sealed partial class BooksPage : Page
    {
        private IServiceScope _scope;

        public BooksPage()
        {
            InitializeComponent();
            // ViewModel.UseNavigation = false;

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            BookDetailUC.ViewModel = _scope.ServiceProvider.GetRequiredService<BookDetailViewModel>();
            ViewModel = _scope.ServiceProvider.GetRequiredService<BooksViewModel>();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public BooksViewModel ViewModel { get; }
    }
}
