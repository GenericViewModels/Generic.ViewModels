using GenericViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Generic.ViewModels.Tests.Services
{
    public class SelectedItemServiceTests
    {
        [Fact]
        public void SetSelectedItemWithChange()
        {
            // arrange
            var service = new SelectedItemService<string>();
            bool firedEvent = false;
            service.SelectedItem = "one";
            service.SelectedItemChanged += (sender, e) =>
            {
                firedEvent = true;
            };
            // act
            service.SelectedItem = "two";
            // assert
            Assert.True(firedEvent);
        }

        [Fact]
        public void SetSelectedItemWithNoChange()
        {
            // arrange
            var service = new SelectedItemService<string>();
            bool firedEvent = false;
            service.SelectedItem = "one";
            service.SelectedItemChanged += (sender, e) =>
            {
                firedEvent = true;
            };
            // act
            service.SelectedItem = "one";
            // assert
            Assert.False(firedEvent);
        }

    }
}
