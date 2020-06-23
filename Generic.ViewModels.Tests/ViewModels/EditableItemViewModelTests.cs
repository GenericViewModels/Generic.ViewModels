using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    public class EditableItemViewModelTests
    {
        public class AnItem
        {
            public string Text { get; set; }
        }
        public class TestEditableItemViewModel : EditableItemViewModel<AnItem>
        {
            public TestEditableItemViewModel(IItemsService<AnItem> itemsService, IShowProgressInfo showProgressInfo, ILoggerFactory loggerFactory)
                : base(itemsService, showProgressInfo, loggerFactory)
            {
            }

            protected override AnItem CreateCopy(AnItem item) => new AnItem() { Text = item?.Text ?? "empty" };
            protected override Task OnAddCoreAsync() => Task.CompletedTask;
            protected override Task OnSaveCoreAsync()
            {
                Item.Text = EditItem.Text;
                return Task.CompletedTask;
            }

            public bool IsDeleted { get; private set; } = false;
            protected override Task OnDeleteCoreAsync()
            {
                IsDeleted = true;
                return Task.CompletedTask;
            }

            public bool SureToDelete { get; set; }
            protected override Task<bool> AreYouSureAsync() => Task.FromResult(SureToDelete);

            protected override void OnEditCommandChanges()
            {
            }
        }

        public EditableItemViewModelTests()
        {
            ObservableCollection<AnItem> items = new ObservableCollection<AnItem>
            {
                new AnItem { Text = "first" },
                new AnItem { Text = "second" },
                new AnItem { Text = "third" }
            };
            var mockingObject = new Mock<IItemsService<AnItem>>();
            mockingObject.Setup(service => service.RefreshAsync())
                .Returns(Task.CompletedTask);
            mockingObject.Setup(service => service.Items)
                .Returns(() => items);
            var selectedItemMockingObject = new Mock<ISharedItems<AnItem>>();
            selectedItemMockingObject.Setup(service => service.SelectedItem)
                .Returns(items[1]);
            _itemsService = mockingObject.Object;
            _selectedItemService = selectedItemMockingObject.Object;

            var mockShowProgress = new Mock<IShowProgressInfo>();
            mockShowProgress.Setup(service => service.StartInProgress("progress1"));
            _showProgressInfo = mockShowProgress.Object;

            var mockLogger = new Mock<ILoggerFactory>();
            _loggerFactory = mockLogger.Object;
        }

        private readonly IItemsService<AnItem> _itemsService;
        private readonly ISharedItems<AnItem> _selectedItemService;
        private readonly IShowProgressInfo _showProgressInfo;
        private readonly ILoggerFactory _loggerFactory;

        [Fact]
        public void FireEditItemEventOnItemChange()
        {
            bool editItemChangedFired = false;
            bool itemChangedFired = false;

            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Item") itemChangedFired = true;
                if (e.PropertyName == "EditItem") editItemChangedFired = true;
            };
            viewModel.Item = new AnItem();

            Assert.True(editItemChangedFired);
            Assert.True(itemChangedFired);
        }

        [Fact]
        public void BeginEdit_IsEditMode()
        {
            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);

            viewModel.BeginEdit();

            Assert.True(viewModel.IsEditMode);
        }

        [Fact]
        public void BeginEdit_CreateCopy()
        {
            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);

            viewModel.BeginEdit();
            viewModel.EditItem.Text = "new text";

            Assert.NotEqual(viewModel.Item.Text, viewModel.EditItem.Text);
        }

        [Fact]
        public void CancelEdit_EditMode()
        {
            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);
            viewModel.BeginEdit();

            viewModel.CancelEdit();

            Assert.False(viewModel.IsEditMode);
        }

        [Fact]
        public void CancelEdit_EditItemReset()
        {
            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);
            viewModel.BeginEdit();
            viewModel.EditItem.Text = "new text";

            viewModel.CancelEdit();

            Assert.Equal("second", viewModel.Item.Text);
            Assert.Equal(viewModel.Item.Text, viewModel.EditItem.Text);
        }

        [Fact]
        public void EndEdit_ItemSaved()
        {
            var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);
            viewModel.BeginEdit();
            viewModel.EditItem.Text = "new text";

            viewModel.EndEdit();

            Assert.Equal("new text", viewModel.EditItem.Text);
            Assert.Equal(viewModel.EditItem.Text, viewModel.Item.Text);
        }

        //[Fact]
        //public void DeleteCommand_DoNothingOnReadMode()
        //{
        //    var viewModel = new TestEditableItemViewModel(_itemsService, _showProgressInfo, _loggerFactory);
        //    viewModel.DeleteCommand.Execute();
        //    Assert.False(viewModel.IsDeleted);
        //}

        //[Fact]
        //public void DeleteCommand_CallOnDeleteAsyncNotSure()
        //{
        //    var viewModel = new TestEditableItemViewModel(_itemsService, _selectedItemService)
        //    {
        //        SureToDelete = false
        //    };
        //    viewModel.DeleteCommand.Execute();
        //    Assert.False(viewModel.IsDeleted);
        //}

        //[Fact]
        //public void DeleteCommand_CallOnDeleteAsyncSure_IsReadModeAfterDelete()
        //{
        //    var viewModel = new TestEditableItemViewModel(_itemsService, _selectedItemService)
        //    {
        //        SureToDelete = true
        //    };
        //    viewModel.DeleteCommand.Execute();
        //    Assert.True(viewModel.IsDeleted);
        //}
    }
}
