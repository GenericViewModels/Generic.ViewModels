using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{

    //public class ItemViewModelTest
    //{
    //    public class Model
    //    {
    //        public string Text { get; set; }
    //    }
    //    public class TestItemViewModel : ItemViewModel<Model>
    //    {
    //        public TestItemViewModel()
    //        {

    //        }

    //        public TestItemViewModel(Model model)
    //            : base(model)
    //        {

    //        }

    //    }

    //    [Fact]
    //    public void SetItemWithNotification()
    //    {
    //        // arrange
    //        var vm = new TestItemViewModel();
    //        var model = new Model { Text = "test" };
    //        bool propertyFired = false;
    //        vm.PropertyChanged += (sender, e) =>
    //        {
    //            if (e.PropertyName == "Item")
    //            {
    //                propertyFired = true;
    //            }
    //        };
    //        // act
    //        vm.Item = model;
    //        // assert
    //        Assert.True(propertyFired);
    //    }

    //    [Fact]
    //    public void ConstructorItemViewModel()
    //    {
    //        // arrange
    //        var model = new Model { Text = "test" };

    //        // act
    //        var vm = new TestItemViewModel(model);

    //        // assert
    //        Assert.Equal(model, vm.Item);
    //    }
    //}
}
