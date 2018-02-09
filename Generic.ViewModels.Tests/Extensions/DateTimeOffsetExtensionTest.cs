using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GenericViewModels.Extensions;


namespace Generic.ViewModels.Tests.Extensions
{
    public class DateTimeOffsetExtensionTest
    {
        [Fact]
        public void GetMondayUsingMonday()
        {
            DateTimeOffset date = new DateTimeOffset(new DateTime(2018, 2, 5));
            DateTimeOffset expected = date;
            DateTimeOffset actual = date.GetMonday();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetMondayUsingWednesday()
        {
            DateTimeOffset date = new DateTimeOffset(new DateTime(2018, 2, 7));
            DateTimeOffset expected = new DateTimeOffset(new DateTime(2018, 2, 5)); 
            DateTimeOffset actual = date.GetMonday();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetNextMondayUsingMonday()
        {
            DateTimeOffset date = new DateTimeOffset(new DateTime(2018, 2, 5));
            DateTimeOffset expected = date.AddDays(7);
            DateTimeOffset actual = date.GetNextMonday();
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
            DateTimeOffset date = new DateTimeOffset(new DateTime(year, month, day));
            DateTimeOffset expected = new DateTimeOffset(new DateTime(expectedYear, expectedMonth, expectedDay));
            DateTimeOffset actual = date.GetNextMonday();
            Assert.Equal(expected, actual);
        }

        private static IEnumerable<object[]> GetDates()
        {

        }

        [Theory]
        [MemberData()]
        [InlineData(2018, 2, 4, 2018, 2, 5)]
        [InlineData(2018, 2, 5, 2018, 2, 12)]
        [InlineData(2018, 2, 6, 2018, 2, 12)]
        [InlineData(2018, 2, 7, 2018, 2, 12)]
        [InlineData(2018, 2, 8, 2018, 2, 12)]
        public void GetNextMonday2(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
        {
            DateTimeOffset date = new DateTimeOffset(new DateTime(year, month, day));
            DateTimeOffset expected = new DateTimeOffset(new DateTime(expectedYear, expectedMonth, expectedDay));
            DateTimeOffset actual = date.GetNextMonday();
            Assert.Equal(expected, actual);
        }
    }
}
