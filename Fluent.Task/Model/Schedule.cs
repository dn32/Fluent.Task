﻿using Fluent.Task.Enum;
using System;

namespace Fluent.Task
{
    public class Schedule
    {
        #region PROPERTIES

        public Action<Schedule> Action { get; private set; }
        public string Name { get; private set; }
        public DateTime DateTime { get; private set; }
        public bool Loop { get; private set; }
        public string Key { get; set; }
        public eStateOfTask State { get; set; }
        private TimeSpan FrequencyOfLoop { get; set; }

        #endregion
        
        public static Schedule Instance()
        {
            return new Schedule();
        }

        public Schedule()
        {
            State = eStateOfTask.NOT_ADDED;
        }

        public Schedule SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public Schedule SetAction(Action<Schedule> action)
        {
            this.Action = action;
            return this;
        }

        public Schedule SetTime(TimeSpan dateTime)
        {
            this.DateTime = DateTime.Now.Add(dateTime);
            this.FrequencyOfLoop = dateTime;
            return this;
        }

        public Schedule SetTime(int seconds)
        {
            this.DateTime = DateTime.Now.AddSeconds(seconds);
            this.FrequencyOfLoop = TimeSpan.FromSeconds(seconds);
            return this;
        }

        public Schedule SetLoop()
        {
            this.Loop = true;
            return this;
        }

        public Schedule Restart()
        {
            this.DateTime = DateTime.Now.Add(this.FrequencyOfLoop);
            this.State = eStateOfTask.WAITING;
            return this;
        }

        public Schedule Run(TaskScheduler taskService)
        {
            taskService.Add(this);
            return this;
        }

        public bool Validade(out string message)
        {
            message = string.Empty;

            if (this.DateTime == new DateTime())
            {
                message += "dateTime is not defined\n";
            }

            if (this.DateTime <= DateTime.Now)
            {
                message += "time in the past is not accepted\n";
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
