using System;

namespace GenericViewModels.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset GetNextMonday(this DateTimeOffset date)
        {
            int addDays = 8 - (int)date.DayOfWeek;
            if (addDays > 7) addDays -= 7;
            return date.AddDays(addDays);
        }

        public static DateTimeOffset GetMonday(this DateTimeOffset date)
        {
            int number = (int)date.DayOfWeek;
            if (number == 0) return date.AddDays(1);
            else if (number == 1) return date;
            else return date.AddDays(-(number - 1));
        }
    }
}
