using Fluent.DateTimeExtensions;
using Fluent.Task.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluent.Task.Model
{
    public class TimeSettings
    {
        public bool IsLoop { get; set; }
        public bool StartImmediately { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan FrequencyOfLoop { get; set; }
        public eFrequencyType FrequencyType { get; set; }
        public DayOfWeek DayOfTheWeeky { get; set; }
        public int? Month { get; set; } = null;
        public int? Day { get; set; } = null;
        public int? Hour { get; set; } = null;
        public int? Minute { get; set; } = null;
        public int? Second { get; set; } = null;

        public TimeSettings SetDateTime(int? month, int? day, int? hour, int? minute, int? second)
        {
            this.Month = month;
            this.Day = day;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
            return this;
        }

        public TimeSettings SetDateTime(DayOfWeek dayOfTheWeeky, int? hour, int? minute, int? second)
        {
            this.FrequencyType = eFrequencyType.WEEKLY;
            this.DayOfTheWeeky = dayOfTheWeeky;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
            return this;
        }

        internal void Calculate()
        {
            if (StartImmediately)
            {
                StartImmediately = false;
                this.DateTime = DateTime.Now;
                return;
            }

            if (FrequencyType == eFrequencyType.BY_INTERVAL)
            {
                DateTime = DateTime.Now.Add(FrequencyOfLoop);
            }
            else if (FrequencyType == eFrequencyType.WEEKLY)
            {
                DateTime = DateTime.Now.GetNextWeekDay(DayOfTheWeeky, Hour, Minute, Second);
            }
            else if (Month != null)
            {
                DateTime = DateTime.Now.GetNextMoth(Month.Value, Day, Hour, Minute, Second);
            }
            else if (Day != null)
            {
                DateTime = DateTime.Now.GetNextDay(Day.Value, Hour, Minute, Second);
            }
            else if (Hour != null)
            {
                DateTime = DateTime.Now.GetNextHour(Hour.Value, Minute, Second);
            }
            else if (Minute != null)
            {
                DateTime = DateTime.Now.GetNextMinute(Minute.Value, Second);
            }
            else
            {
                DateTime = DateTime.Now.GetNextSecond(Second.Value);
            }

#if DEBUG
            Console.WriteLine($"Schedule added for {DateTime}");
#endif
        }
    }
}

