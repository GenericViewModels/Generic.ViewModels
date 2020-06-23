using GenericViewModels.Services;
using GenericViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Generic.ViewModels.Tests.ViewModels
{
    public class ShowProgressInfoTest
    {
        public class Model
        {
            public string Text { get; set; }
        }

        [Fact]
        public void TestInProgress()
        {
            // arrange
            var info = new ShowProgressInfo();
            bool inProgressShouldBeTrue = false;
            bool inProgressShouldBeFalse = true;
            string action = "firstAction";
            // act
            using (info.StartInProgress(action))
            {
                inProgressShouldBeTrue = info.InProgress(action);
            }
            inProgressShouldBeFalse = info.InProgress(action);

            // assert
            Assert.True(inProgressShouldBeTrue);
            Assert.False(inProgressShouldBeFalse);
        }

        [Fact]
        public void MultiCountInProgress()
        {
            // arrange
            var info = new ShowProgressInfo();
            bool inProgressShouldBeTrue = false;
            bool inProgressShouldStillBeTrue = false;
            bool inProgressShouldBeFalse = true;
            string action = "Action";
            // act
            using (info.StartInProgress(action))
            {
                using (info.StartInProgress(action))
                {
                    inProgressShouldBeTrue = info.InProgress(action);
                }
                inProgressShouldStillBeTrue = info.InProgress(action);
            }
            inProgressShouldBeFalse = info.InProgress(action);

            // assert
            Assert.True(inProgressShouldBeTrue);
            Assert.True(inProgressShouldStillBeTrue);
            Assert.False(inProgressShouldBeFalse);
        }
    }
}
