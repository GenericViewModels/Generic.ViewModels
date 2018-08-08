using BooksLib.Models;
using GenericViewModels.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksLib.Services
{
    public class BooksService : ItemsService<Book>
    {
        private IBooksRepository _booksRepository;
        private ILogger _logger;

        public BooksService(
            IBooksRepository booksRepository,
            ISharedItems<Book> booksItems, 
            ILoggerFactory loggerFactory)
            : base(booksItems, loggerFactory)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _logger = loggerFactory?.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public override async Task<Book> AddOrUpdateAsync(Book book)
        {
            Book updated = null;
            if (book.BookId == 0)
            {
                updated = await _booksRepository.AddAsync(book);
            }
            else
            {
                updated = await _booksRepository.UpdateAsync(book);
            }
            return updated;
        }

        public override Task DeleteAsync(Book book) =>
            _booksRepository.DeleteAsync(book.BookId);

        public override async Task RefreshAsync()
        {
            IEnumerable<Book> books = await _booksRepository.GetItemsAsync();
            Items.Clear();

            foreach (var book in books)
            {
                Items.Add(book);
            }
           
            SelectedItem = Items.FirstOrDefault();
            base.RefreshAsync();
        }
    }
}
