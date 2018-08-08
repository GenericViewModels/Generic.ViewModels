﻿using BooksLib.Models;
using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Prism.Commands;
using System;

namespace BooksLib.ViewModels
{
    // this is the view model display book items within a list
    public class BookItemViewModel : ItemViewModel<Book>
    {
        private readonly IItemsService<Book> _booksService;

        public BookItemViewModel(
            Book book, 
            IItemsService<Book> booksService,
            IShowProgressInfo showProgressInfo)
            : base(book, showProgressInfo)
        {
            _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));

            DeleteBookCommand = new DelegateCommand(OnDeleteBook);
        }

        public DelegateCommand DeleteBookCommand { get; set; }

        private async void OnDeleteBook()
        {
            await _booksService.DeleteAsync(Item);
        }
    }
}
