using BooksApp.Services;
using BooksApp.Views;
using BooksLib.Services;
using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace BooksApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Dictionary<string, Type> _pages = new Dictionary<string, Type>
        {
            [PageNames.BooksPage] = typeof(BooksPage),
            [PageNames.BookDetailPage] = typeof(BookDetailPage)
        };

        private readonly INavigationService _navigationService;
        private readonly UWPInitializeNavigationService _initializeNavigationService;
        public MainPageViewModel(INavigationService navigationService, UWPInitializeNavigationService initializeNavigationService)
        {
            _navigationService = navigationService;
            _initializeNavigationService = initializeNavigationService;

            SystemNavigationManager navigationManager = SystemNavigationManager.GetForCurrentView();

            navigationManager.BackRequested += OnBackRequested;
            navigationManager.AppViewBackButtonVisibility = _navigationService.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public void SetNavigationFrame(Frame frame) => _initializeNavigationService.Initialize(frame, _pages);

        public void OnNavigationSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem navigationItem)
            {
                switch (navigationItem.Tag)
                {
                    case "books":
                        _navigationService.NavigateToAsync(PageNames.BooksPage);
                        break;
                    default:
                        break;
                }
            }
        }

        public async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (_navigationService.CanGoBack)
            {
                e.Handled = true;
                await _navigationService.GoBackAsync();
            }
        }
    }
}
