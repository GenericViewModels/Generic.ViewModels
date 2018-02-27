using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    public class ViewModelBaseTest
    {
        public class Model
        {
            public string Text { get; set; }
        }
        public class TestViewModel : ViewModelBase
        {

        }

        [Fact]
        public void TestInProgress()
        {
            // arrange
            var vm = new TestViewModel();
            bool inProgressShouldBeTrue = false;
            bool inProgressShouldBeFalse = true;
            // act
            using (vm.StartInProgress())
            {
                inProgressShouldBeTrue = vm.InProgress;
            }
            inProgressShouldBeFalse = vm.InProgress;

            // assert
            Assert.True(inProgressShouldBeTrue);
            Assert.False(inProgressShouldBeFalse);
        }

        [Fact]
        public void MultiCountInProgress()
        {
            // arrange
            var vm = new TestViewModel();
            bool inProgressShouldBeTrue = false;
            bool inProgressShouldStillBeTrue = false;
            bool inProgressShouldBeFalse = true;
            // act
            using (vm.StartInProgress())
            {
                using (vm.StartInProgress())
                {
                    inProgressShouldBeTrue = vm.InProgress;
                }
                inProgressShouldStillBeTrue = vm.InProgress;
            }
            inProgressShouldBeFalse = vm.InProgress;

            // assert
            Assert.True(inProgressShouldBeTrue);
            Assert.True(inProgressShouldStillBeTrue);
            Assert.False(inProgressShouldBeFalse);
        }
    }
}
