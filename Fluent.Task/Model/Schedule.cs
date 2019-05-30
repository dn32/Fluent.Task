using FluentTask.Enum;
using FluentTask.Model;
using System;

namespace FluentTask
{
    public class Schedule
    {
        #region PROPERTIES

        public Action<object> Action { get; private set; }
        public string Name { get; private set; }
        public eStateOfTask State { get; set; }
        public object Parameter { get; private set; }
        public TimeSettings LoopSettings { get; set; }

        #endregion

        public static Schedule Instance(Action<object> action)
        {
            return new Schedule(action);
        }

        public Schedule(Action<object> action)
        {
            this.Name = Guid.NewGuid().ToString();
            this.State = eStateOfTask.NOT_ADDED;
            this.Action = action;
            this.LoopSettings = new TimeSettings { FrequencyType = eFrequencyType.NULL };
        }

        public Schedule SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public Schedule SetParameter(object parameter)
        {
            this.Parameter = parameter;
            return this;
        }

        public Schedule SetAction(Action<object> action)
        {
            this.Action = action;
            return this;
        }

        public Schedule SetDateTime(int month, int day = 0, int hour = 0, int minute = 0, int second = 0)
        {
            this.LoopSettings.SetDateTime(month, day, hour, minute, second);
            return this;
        }

        public Schedule SetDateTime(DayOfWeek dayOfTheWeeky, int hour = 0, int minute = 0, int second = 0)
        {
            this.LoopSettings.SetDateTime(dayOfTheWeeky, hour, minute, second);
            return this;
        }

        public Schedule SetTimeWeekly(DayOfWeek dayOfTheWeeky)
        {
            this.LoopSettings.FrequencyType = eFrequencyType.WEEKLY;
            this.LoopSettings.DayOfTheWeeky = dayOfTheWeeky;
            return this;
        }

        public Schedule SetTimeMonthly(int month)
        {
            this.LoopSettings.Month = month;
            return this;
        }

        public Schedule SetTimeDay(int day)
        {
            this.LoopSettings.Day = day;
            return this;
        }

        public Schedule SetTimeHour(int hour)
        {
            this.LoopSettings.Hour = hour;
            return this;
        }

        public Schedule SetTimeMinute(int minute)
        {
            this.LoopSettings.Minute = minute;
            return this;
        }

        public Schedule SetTimeSecond(int second)
        {
            this.LoopSettings.Second = second;
            return this;
        }

        public Schedule SetStartImmediately()
        {
            this.LoopSettings.StartImmediately = true;
            return this;
        }

        public Schedule Restart()
        {
            this.LoopSettings.Calculate(this);
            this.State = eStateOfTask.WAITING;
            return this;
        }

        public Schedule SetFrequencyTime(TimeSpan time)
        {
            this.LoopSettings.FrequencyType = eFrequencyType.BY_INTERVAL;
            this.LoopSettings.FrequencyOfLoop = time;
            return this;
        }

        public Schedule SetFrequencyTime(int seconds)
        {
            this.LoopSettings.FrequencyType = eFrequencyType.BY_INTERVAL;
            this.LoopSettings.FrequencyOfLoop = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public Schedule Run(TaskScheduler taskService)
        {
            this.LoopSettings.Calculate(this);
            taskService.Add(this);
            return this;
        }

        public Schedule RunLoop(TaskScheduler taskService)
        {
            this.LoopSettings.IsLoop = true;
            return Run(taskService);
        }

        public bool Validade(out string message)
        {
            message = string.Empty;

            if (this.LoopSettings.DateTime == new DateTime())
            {
                message += "dateTime is not defined\n";
            }

            if (this.Action == null)
            {
                message += "action is not defined\n";
            }

            if (this.Name == null)
            {
                message += "name is not defined\n";
            }

            if (string.IsNullOrEmpty(message))
            {
                message += "sucess";
                return true;
            }

            return false;
        }
    }
}