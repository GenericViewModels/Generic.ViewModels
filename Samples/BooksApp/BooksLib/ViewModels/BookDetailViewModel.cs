using BooksLib.Models;
using BooksLib.Services;
using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BooksLib.ViewModels
{
    // this view model is used to display details of a book and allows editing
    public class BookDetailViewModel : EditableItemViewModel<Book>, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;
        public BookDetailViewModel(
            IItemsService<Book> itemsService, 
            INavigationService navigationService, 
            IMessageService messageService, 
            IShowProgressInfo showProgressInfo,
            ILoggerFactory loggerFactory)
            : base(itemsService, showProgressInfo, loggerFactory)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));

            _itemsService.SelectedItemChanged += ItemsService_SelectedItemChanged;
        }

        public override void Dispose()
        {
            _itemsService.SelectedItemChanged -= ItemsService_SelectedItemChanged;
        }

        private void ItemsService_SelectedItemChanged(object sender, SelectedItemEventArgs<Book> e)
        {
            Item = e.Item;
        }

        public bool UseNavigation { get; set; }

        protected override Book CreateCopy(Book item) =>
            new Book
            {
                BookId = item?.BookId ?? -1,
                Title = item?.Title ?? "enter a title",
                Publisher = item?.Publisher ?? "enter a publisher"
            };

        protected async override Task OnSaveCoreAsync()
        {
            try
            {
                await _itemsService.AddOrUpdateAsync(EditItem);
            }
            catch (Exception ex)
            {
                _logger.LogError("error {0} in {1}", ex.Message, nameof(OnSaveCoreAsync));
                await _messageService.ShowMessageAsync("Error saving the data");
            }
        }

        protected async override Task OnEndEditAsync()
        {
            if (UseNavigation)
            {
                await _navigationService.GoBackAsync();
            }
        }

        protected override Task OnDeleteCoreAsync() => throw new NotImplementedException();
    }
}
