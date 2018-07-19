using System;
using System.Collections.Generic;
using System.Text;

namespace GenericViewModels.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// returns the monday of the next week of the supplied date
        /// </summary>
        /// <param name="date">A date <see cref="DateTime"/></param>
        /// <returns>the monday of the next week of the supplied date</returns>
        public static DateTime GetNextMonday(this DateTime date)
        {
            int addDays = 8 - (int)date.DayOfWeek;
            if (addDays > 7) addDays -= 7;
            return date.AddDays(addDays);
        }

        /// <summary>
        /// returns the monday of the current week of the supplied date
        /// </summary>
        /// <param name="date">A date <see cref="DateTime"/></param>
        /// <returns>the monday of the current week of the supplied date</returns>
        public static DateTime GetMonday(this DateTime date)
        {
            int number = (int)date.DayOfWeek;
            if (number == 0) return date.AddDays(1);
            else if (number == 1) return date;
            else return date.AddDays(-(number - 1));
        }
    }
}
