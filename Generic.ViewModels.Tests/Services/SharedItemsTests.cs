using GenericViewModels.Services;
using Xunit;

namespace Generic.ViewModels.Tests.Services
{
    public class SharedItemsTests
    {
        [Fact]
        public void SetSelectedItemWithChange()
        {
            // arrange
            var sharedItems = new SharedItems<string>();
            bool firedEvent = false;
            sharedItems.SetSelectedItem("one");
            sharedItems.SelectedItemChanged += (sender, e) =>
            {
                firedEvent = true;
            };
            // act
            sharedItems.SetSelectedItem("two");
            // assert
            Assert.True(firedEvent);
        }

        [Fact]
        public void SetSelectedItemWithNoChange()
        {
            // arrange
            var sharedItems = new SharedItems<string>();
            bool firedEvent = false;
            sharedItems.SetSelectedItem("one");
            sharedItems.SelectedItemChanged += (sender, e) =>
            {
                firedEvent = true;
            };
            // act
            sharedItems.SetSelectedItem("one");
            // assert
            Assert.False(firedEvent);
        }

    }
}
