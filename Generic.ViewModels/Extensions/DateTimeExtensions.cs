using System;
using System.Collections.Generic;
using System.Text;

namespace GenericViewModels.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNextMonday(this DateTime date)
        {
            int addDays = 8 - (int)date.DayOfWeek;
            if (addDays > 7) addDays -= 7;
            return date.AddDays(addDays);
        }

        public static DateTime GetMonday(this DateTime date)
        {
            int number = (int)date.DayOfWeek;
            if (number == 0) return date.AddDays(1);
            else if (number == 1) return date;
            else return date.AddDays(-(number - 1));
        }
    }
}
