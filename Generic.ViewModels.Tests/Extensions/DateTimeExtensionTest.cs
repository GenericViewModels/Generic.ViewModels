using GenericViewModels.Extensions;
using System;
using Xunit;


namespace Generic.ViewModels.Tests.Extensions
{
    public class DateTimeExtensionTest
    {
        [Fact]
        public void GetMondayUsingMonday()
        {
            DateTime date = new DateTime(2018, 2, 5);
            DateTime expected = date;
            DateTime actual = date.GetMonday();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMondayUsingWednesday()
        {
            DateTime date = new DateTime(2018, 2, 7);
            DateTime expected = new DateTime(2018, 2, 5);
            DateTime actual = date.GetMonday();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNextMondayUsingMonday()
        {
            DateTime date = new DateTime(2018, 2, 5);
            DateTime expected = date.AddDays(7);
            DateTime actual = date.GetNextMonday();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2018, 2, 4, 2018, 2, 5)]
        [InlineData(2018, 2, 5, 2018, 2, 12)]
        [InlineData(2018, 2, 6, 2018, 2, 12)]
        [InlineData(2018, 2, 7, 2018, 2, 12)]
        [InlineData(2018, 2, 8, 2018, 2, 12)]
        public void GetNextMonday(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
        {
            DateTime date = new DateTime(year, month, day);
            DateTime expected = new DateTime(expectedYear, expectedMonth, expectedDay);
            DateTime actual = date.GetNextMonday();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2018, 2, 4, 2018, 2, 5)]
        [InlineData(2018, 2, 5, 2018, 2, 12)]
        [InlineData(2018, 2, 6, 2018, 2, 12)]
        [InlineData(2018, 2, 7, 2018, 2, 12)]
        [InlineData(2018, 2, 8, 2018, 2, 12)]
        public void GetNextMonday2(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
        {
            DateTime date = new DateTime(year, month, day);
            DateTime expected = new DateTime(expectedYear, expectedMonth, expectedDay);
            DateTime actual = date.GetNextMonday();
            Assert.Equal(expected, actual);
        }
    }
}
