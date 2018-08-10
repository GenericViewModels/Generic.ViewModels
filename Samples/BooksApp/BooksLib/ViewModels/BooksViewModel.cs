using BooksLib.Events;
using BooksLib.Models;
using BooksLib.Services;
using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace BooksLib.ViewModels
{
    public class BooksViewModel : MasterDetailViewModel<BookItemViewModel, Book>
    {
        private readonly IItemsService<Book> _booksService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public BooksViewModel(IItemsService<Book> booksService, 
            INavigationService navigationService,
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory,
            IEventAggregator eventAggregator)
            : base(booksService, showProgressInfo, loggerFactory)
        {
            _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<UseNavigationEvent>().Subscribe(useNavigation =>
            {
                _navigationService.UseNavigation = useNavigation;
            });

            PropertyChanged += async (sender, e) =>
            {
                if (_navigationService.UseNavigation && e.PropertyName == nameof(SelectedItem) && _navigationService.CurrentPage == PageNames.BooksPage)
                {
                    await _navigationService.NavigateToAsync(PageNames.BookDetailPage);
                }
            };
        }

        protected override Task OnAddCoreAsync()
        {
            var newBook = new Book();
            _itemsService.Items.Add(newBook);
            _itemsService.SelectedItem = newBook;
            return base.OnRefreshCoreAsync();
        }

        protected override BookItemViewModel ToViewModel(Book item) => new BookItemViewModel(item, _booksService, _showProgressInfo);
    }
}
