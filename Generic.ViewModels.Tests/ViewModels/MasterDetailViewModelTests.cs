using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    //public class MasterDetailViewModelTests
    //{
    //    public class AnItem
    //    {
    //        public string Text { get; set; }
    //    }
    //    public class AnItemViewModel : ItemViewModel<AnItem>
    //    {
    //        public AnItemViewModel(IShowProgressInfo showProgressInfo)
    //            : base(showProgressInfo)
    //        {

    //        }

    //    }

    //    class MockItemsService : IItemsService<AnItem>
    //    {
    //        public MockItemsService(IEnumerable<AnItem> items)
    //        {
    //            Items = new ObservableCollection<AnItem>(items);
    //        }
    //        public ObservableCollection<AnItem> Items { get; }

    //        public AnItem SelectedItem => throw new NotImplementedException();

    //        public bool IsEditMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //        public event EventHandler<EventArgs> ItemsRefreshed;
    //        public event EventHandler<SelectedItemEventArgs<AnItem>> SelectedItemChanged;
    //        public event PropertyChangedEventHandler PropertyChanged;

    //        public Task<AnItem> AddOrUpdateAsync(AnItem item) => throw new NotImplementedException();
    //        public Task DeleteAsync(AnItem item) => throw new NotImplementedException();
    //        public Task RefreshAsync() => Task.CompletedTask;

    //        public bool? SetSelectedItem(AnItem item)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class MockSharedItems : ISharedItems<AnItem>
    //    {
    //        private AnItem _selectedItem;
    //        public AnItem SelectedItem
    //        {
    //            get => _selectedItem;
    //            set
    //            {
    //                if (_selectedItem != value)
    //                {
    //                    _selectedItem = value;
    //                    SelectedItemChanged?.Invoke(this, _selectedItem);
    //                }
    //            }
    //        }

    //        public ObservableCollection<AnItem> Items => throw new NotImplementedException();

    //        public bool IsEditMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //        public event EventHandler<AnItem> SelectedItemChanged;
    //        public event EventHandler<EventArgs> ItemsRefreshed;
    //        public event PropertyChangedEventHandler PropertyChanged;

    //        event EventHandler<SelectedItemEventArgs<AnItem>> ISharedItems<AnItem>.SelectedItemChanged
    //        {
    //            add
    //            {
    //                throw new NotImplementedException();
    //            }

    //            remove
    //            {
    //                throw new NotImplementedException();
    //            }
    //        }

    //        public void RaiseItemsRefreshed()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public bool? SetSelectedItem(AnItem item)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public class TestMasterDetailViewModel : MasterDetailViewModel<AnItemViewModel, AnItem>
    //    {
    //        public TestMasterDetailViewModel(IItemsService<AnItem> itemsService, ISelectedItemService<AnItem> selectedItemService)
    //            : base(itemsService, selectedItemService)
    //        {
    //        }

    //        protected override Task OnAddCoreAsync() => Task.CompletedTask;
    //        protected override AnItemViewModel ToViewModel(AnItem item) => new AnItemViewModel() { Item = item };
    //    }

    //    public MasterDetailViewModelTests()
    //    {
    //        _itemsService = new MockItemsService(new List<AnItem>() { _item1, _item2, _item3 });
    //        _sharedItems = new MockSharedItems();
    //        _sharedItems.SetSelectedItem(_item2);
    //    }

    //    private AnItem _item1 = new AnItem { Text = "first" };
    //    private AnItem _item2 = new AnItem { Text = "second" };
    //    private AnItem _item3 = new AnItem { Text = "third" };
    //    private IItemsService<AnItem> _itemsService;
    //    private ISharedItems<AnItem> _sharedItems;

    //    [Fact]
    //    public void SetSelectedItem_RaiseEvents()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);
    //        bool selectedItemChangedFired = false;
    //        bool selectedItemViewModelChangedFired = false;
    //        viewModel.PropertyChanged += (sender, e) =>
    //        {
    //            if (e.PropertyName == "SelectedItem") selectedItemChangedFired = true;
    //            if (e.PropertyName == "SelectedItemViewModel") selectedItemViewModelChangedFired = true;
    //        };

    //        viewModel.SelectedItem = _item1;

    //        Assert.True(selectedItemChangedFired);
    //        Assert.True(selectedItemViewModelChangedFired);
    //    }

    //    [Fact]
    //    public void SetSelectedItem_DoNothing()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);
    //        bool selectedItemChangedFired = false;
    //        bool selectedItemViewModelChangedFired = false;
    //        viewModel.PropertyChanged += (sender, e) =>
    //        {
    //            if (e.PropertyName == "SelectedItem") selectedItemChangedFired = true;
    //            if (e.PropertyName == "SelectedItemViewModel") selectedItemViewModelChangedFired = true;
    //        };

    //        viewModel.SelectedItem = _item2;

    //        Assert.False(selectedItemChangedFired);
    //        Assert.False(selectedItemViewModelChangedFired);
    //    }

    //    [Fact]
    //    public void GetSelectedItemViewModel()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);

    //        var vm = viewModel.SelectedItemViewModel;

    //        Assert.Equal(_item2, vm.Item);
    //    }

    //    [Fact]
    //    public void SetSelectedItemViewModel_SetSelectedItem()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);
    //        viewModel.SelectedItemViewModel = new AnItemViewModel() { Item = _item3 };

    //        Assert.Equal(_item3, viewModel.SelectedItem);
    //    }

    //    [Fact]
    //    public void OnRefresh_SetSelectedItem()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);
    //        viewModel.RefreshCommand.Execute();

    //        Assert.Equal(_item1, viewModel.SelectedItem);
    //    }

    //    [Fact]
    //    public async Task InitAsync_CallRefresh()
    //    {
    //        var viewModel = new TestMasterDetailViewModel(_itemsService, _sharedItems);
    //        await viewModel.InitAsync();

    //        Assert.Equal(_item1, viewModel.SelectedItem);
    //    }
    //}
}
