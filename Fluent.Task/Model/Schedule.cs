using Fluent.Task.Enum;
using Fluent.Task.Model;
using System;

namespace Fluent.Task
{
    public class Schedule
    {
        #region PROPERTIES

        public Action<Schedule> Action { get; private set; }
        public string Name { get; private set; }
        public eStateOfTask State { get; set; }
        public Object AditionalParameter { get; private set; }
        public TimeSettings LoopSettings { get; set; }

        #endregion

        public static Schedule Instance(Action<Schedule> action)
        {
            return new Schedule(action);
        }

        public Schedule(Action<Schedule> action)
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

        public Schedule SetAditionalParameter(object aditionalParameter)
        {
            this.AditionalParameter = aditionalParameter;
            return this;
        }

        public Schedule SetAction(Action<Schedule> action)
        {
            this.Action = action;
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
            this.LoopSettings.Calculate();
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
            this.LoopSettings.Calculate();
            taskService.Add(this);
            return this;
        }

        public Schedule RunLoop(TaskScheduler taskService)
        {
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