using BooksLib.Models;
using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Prism.Commands;

namespace BooksLib.ViewModels
{
    // this is the view model display book items within a list
    public class BookItemViewModel : ItemViewModel<Book>
    {
        private readonly IItemsService<Book> _booksService;

        public BookItemViewModel(Book book, IItemsService<Book> booksService)
            : base(book)
        {
            _booksService = booksService;
            DeleteBookCommand = new DelegateCommand(OnDeleteBook);
        }

        public DelegateCommand DeleteBookCommand { get; set; }

        private async void OnDeleteBook()
        {
            await _booksService.DeleteAsync(Item);
        }
    }
}
