using Fluent.Task.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Fluent.Task.Util;

namespace Fluent.Task.Model
{
    public class TimeSettings
    {
        public bool StartImmediately { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan FrequencyOfLoop { get; set; }
        public eFrequencyType FrequencyType { get; set; }
        public DayOfWeek DayOfTheWeeky { get; set; }

        public int Month { get; set; } = -1;
        public int Day { get; set; } = -1;
        public int Hour { get; set; } = -1;
        public int Minute { get; set; } = -1;
        public int Second { get; set; } = -1;

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
            else if (Month > 0)
            {
                DateTime = DateTime.Now.GetNextMoth(Month, Day, Hour, Minute, Second);
            }
            else if (Day > 0)
            {
                DateTime = DateTime.Now.GetNextDay(Day, Hour, Minute, Second);
            }
            else if (Hour > 0)
            {
                DateTime = DateTime.Now.GetNextHour(Hour, Minute, Second);
            }
            else if (Minute > 0)
            {
                DateTime = DateTime.Now.GetNextMinute(Minute, Second);
            }
            else
            {
                DateTime = DateTime.Now.GetNextSecond(Second);
            }
        }
    }
}

