using System;
using System.Collections.Generic;
using System.Text;

namespace Fluent.Task.Util
{
    /// <summary>
    /// Gets the next occurrence for a specific date and time.
    /// </summary>
    public static class NextDateTimeExtensions
    {
        /// <summary>
        /// Get the next date time of specified month.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextMoth(this DateTime dt, int month, int day = -1, int hour = -1, int min = -1, int sec = -1)
        {
            month = month == -1 ? dt.Month : month;
            day = day == -1 ? dt.Day : day;
            hour = hour == -1 ? dt.Hour : hour;
            min = min == -1 ? dt.Minute : min;
            sec = sec == -1 ? dt.Second : sec;
            min = min > 59 ? 59 : min;
            sec = sec > 59 ? 59 : sec;
            hour = hour > 23 ? 23 : hour;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            day = day > DateTime.DaysInMonth(dt.Year, dt.Month) ? DateTime.DaysInMonth(dt.Year, dt.Month) : day;
            var date = new DateTime(dt.Year, month, day, hour, min, sec);
            return date.DateIsEarlier(dt) ? date.AddYears(1) : date;
        }

        /// <summary>
        /// Get the next date time of specified day.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextDay(this DateTime dt, int day, int hour = -1, int min = -1, int sec = -1)
        {
            day = day == -1 ? dt.Day : day;
            hour = hour == -1 ? dt.Hour : hour;
            min = min == -1 ? dt.Minute : min;
            sec = sec == -1 ? dt.Second : sec;
            min = min > 59 ? 59 : min;
            sec = sec > 59 ? 59 : sec;
            hour = hour > 23 ? 23 : hour;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            day = day > DateTime.DaysInMonth(dt.Year, dt.Month) ? DateTime.DaysInMonth(dt.Year, dt.Month) : day;
            var date = new DateTime(dt.Year, dt.Month, day, hour, min, sec);
            return date.DateIsEarlier(dt) ? date.AddMonths(1) : date;
        }

        /// <summary>
        /// Get the next date time of specified day of week.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextWeekDay(this DateTime dt, DayOfWeek dayOfWeek, int hour = -1, int min = -1, int sec = -1)
        {
            hour = hour == -1 ? dt.Hour : hour;
            min = min == -1 ? dt.Minute : min;
            sec = sec == -1 ? dt.Second : sec;
            min = min > 59 ? 59 : min;
            sec = sec > 59 ? 59 : sec;
            hour = hour > 23 ? 23 : hour;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            var dtNew = new DateTime(dt.Year, dt.Month, dt.Day, hour, min, sec);
            var daysUntilTuesday = (dayOfWeek.GetHashCode() - (int)dtNew.DayOfWeek + 7) % 7;
            var date = dtNew.AddDays(daysUntilTuesday);
            return date.DateIsEarlier(dt) ? date.AddDays(7) : date;
        }

        /// <summary>
        /// Get the next date time of specified hour.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextHour(this DateTime dt, int hour, int min = -1, int sec = -1)
        {
            hour = hour == -1 ? dt.Hour : hour;
            min = min == -1 ? dt.Minute : min;
            sec = sec == -1 ? dt.Second : sec;
            min = min > 59 ? 59 : min;
            sec = sec > 59 ? 59 : sec;
            hour = hour > 23 ? 23 : hour;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            var date = new DateTime(dt.Year, dt.Month, dt.Day, hour, min, sec);
            return date.DateIsEarlier(dt) ? date.AddDays(1) : date;
        }

        /// <summary>
        /// Get the next date time of specified minute.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextMinute(this DateTime dt, int min, int sec = -1)
        {
            min = min == -1 ? dt.Minute : min;
            sec = sec == -1 ? dt.Second : sec;
            min = min > 59 ? 59 : min;
            sec = sec > 59 ? 59 : sec;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            var date = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, min, sec);
            return date.DateIsEarlier(dt) ? date.AddHours(1) : date;
        }

        /// <summary>
        /// Get the next date time of specified second.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static DateTime GetNextSecond(this DateTime dt, int sec)
        {
            sec = sec == -1 ? dt.Second : sec;
            sec = sec > 59 ? 59 : sec;
            dt = dt.AddMilliseconds(-1 * dt.Millisecond);

            var date = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, sec);
            return date.DateIsEarlier(dt) ? date.AddMinutes(1) : date;
        }

        private static bool DateIsEarlier(this DateTime dt1, DateTime dt2)
        {
            return new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, dt1.Second) < new DateTime(dt2.Year, dt2.Month, dt2.Day, dt2.Hour, dt2.Minute, dt2.Second);
        }
    }
}
