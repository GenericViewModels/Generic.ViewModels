using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    // TODO: tests moved to ShowProgressInfoTests; add tests for ViewModelBase

    //public class ViewModelBaseTest
    //{
    //    public class Model
    //    {
    //        public string Text { get; set; }
    //    }
    //    public class TestViewModel : ViewModelBase
    //    {
    //        public TestViewModel(IShowProgressInfo showProgressInfo)
    //            : base(showProgressInfo)
    //        {

    //        }
    //    }

    //    public ViewModelBaseTest()
    //    {
    //        var mockShowProgress = new Mock<IShowProgressInfo>();
    //        _showProgressInfo = mockShowProgress.Object;
    //    }

    //    private readonly IShowProgressInfo _showProgressInfo;

    //    [Fact]
    //    public void TestInProgress()
    //    {
    //        // arrange
    //        var vm = new TestViewModel(_showProgressInfo);
    //        bool inProgressShouldBeTrue = false;
    //        bool inProgressShouldBeFalse = true;
    //        // act
    //        using var progress = vm.InitAsync()
    //        using (vm.StartInProgress())
    //        {
    //            inProgressShouldBeTrue = vm.InProgress;
    //        }
    //        inProgressShouldBeFalse = vm.InProgress;

    //        // assert
    //        Assert.True(inProgressShouldBeTrue);
    //        Assert.False(inProgressShouldBeFalse);
    //    }

    //    [Fact]
    //    public void MultiCountInProgress()
    //    {
    //        // arrange
    //        var vm = new TestViewModel();
    //        bool inProgressShouldBeTrue = false;
    //        bool inProgressShouldStillBeTrue = false;
    //        bool inProgressShouldBeFalse = true;
    //        // act
    //        using (vm.StartInProgress())
    //        {
    //            using (vm.StartInProgress())
    //            {
    //                inProgressShouldBeTrue = vm.InProgress;
    //            }
    //            inProgressShouldStillBeTrue = vm.InProgress;
    //        }
    //        inProgressShouldBeFalse = vm.InProgress;

    //        // assert
    //        Assert.True(inProgressShouldBeTrue);
    //        Assert.True(inProgressShouldStillBeTrue);
    //        Assert.False(inProgressShouldBeFalse);
    //    }
    //}
}
