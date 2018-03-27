using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    public class MasterDetailViewModelTests
    {
        public class AnItem
        {
            public string Text { get; set; }
        }
        public class AnItemViewModel : ItemViewModel<AnItem>
        {

        }

        class MockItemsService : IItemsService<AnItem>
        {
            public MockItemsService(IEnumerable<AnItem> items)
            {
                Items = new ObservableCollection<AnItem>(items);
            }
            public ObservableCollection<AnItem> Items { get; }

            public AnItem SelectedItem { get; set; }

            public event EventHandler<AnItem> SelectedItemChanged;

            public Task<AnItem> AddOrUpdateAsync(AnItem item) => throw new NotImplementedException();
            public Task DeleteAsync(AnItem item) => throw new NotImplementedException();
            public Task RefreshAsync() => Task.CompletedTask;
        }

        public class TestMasterDetailViewModel : MasterDetailViewModel<AnItemViewModel, AnItem>
        {
            public TestMasterDetailViewModel(IItemsService<AnItem> itemsService)
                : base(itemsService)
            {
            }

            protected override Task OnAddCoreAsync() => Task.CompletedTask;
            protected override AnItemViewModel ToViewModel(AnItem item) => new AnItemViewModel() { Item = item };
        }

        public MasterDetailViewModelTests()
        {
            _itemsService = new MockItemsService(new List<AnItem>() { _item1, _item2, _item3 });
            _itemsService.SelectedItem = _item2;
        }

        private AnItem _item1 = new AnItem { Text = "first" };
        private AnItem _item2 = new AnItem { Text = "second" };
        private AnItem _item3 = new AnItem { Text = "third" };
        private IItemsService<AnItem> _itemsService;

        [Fact]
        public void SetSelectedItem_RaiseEvents()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);
            bool selectedItemChangedFired = false;
            bool selectedItemViewModelChangedFired = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedItem") selectedItemChangedFired = true;
                if (e.PropertyName == "SelectedItemViewModel") selectedItemViewModelChangedFired = true;
            };

            viewModel.SelectedItem = _item1;

            Assert.True(selectedItemChangedFired);
            Assert.True(selectedItemViewModelChangedFired);
        }

        [Fact]
        public void SetSelectedItem_DoNothing()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);
            bool selectedItemChangedFired = false;
            bool selectedItemViewModelChangedFired = false;
            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "SelectedItem") selectedItemChangedFired = true;
                if (e.PropertyName == "SelectedItemViewModel") selectedItemViewModelChangedFired = true;
            };

            viewModel.SelectedItem = _item2;

            Assert.False(selectedItemChangedFired);
            Assert.False(selectedItemViewModelChangedFired);
        }

        [Fact]
        public void GetSelectedItemViewModel()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);

            var vm = viewModel.SelectedItemViewModel;

            Assert.Equal(_item2, vm.Item);
        }

        [Fact]
        public void SetSelectedItemViewModel_SetSelectedItem()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);
            viewModel.SelectedItemViewModel = new AnItemViewModel() { Item = _item3 };

            Assert.Equal(_item3, viewModel.SelectedItem);
        }

        [Fact]
        public void OnRefresh_SetSelectedItem()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);
            viewModel.RefreshCommand.Execute();

            Assert.Equal(_item1, viewModel.SelectedItem);
        }

        [Fact]
        public async Task InitAsync_CallRefresh()
        {
            var viewModel = new TestMasterDetailViewModel(_itemsService);
            await viewModel.InitAsync();

            Assert.Equal(_item1, viewModel.SelectedItem);
        }
    }
}
