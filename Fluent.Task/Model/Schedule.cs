using Fluent.Task.Enum;
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
        public eStateOfTask State { get; set; }
        public Object AditionalParameter { get; private set; }
        private TimeSpan FrequencyOfLoop { get; set; }

        #endregion

        public static Schedule Instance(Action<Schedule> action)
        {
            return new Schedule(action);
        }

        public Schedule(Action<Schedule> action)
        {
            Name = Guid.NewGuid().ToString();
            State = eStateOfTask.NOT_ADDED;
            this.Action = action;
        }

        public Schedule SetName(string name)
        {
            this.Name = name;
            return this;
        }

        public Schedule SetAditionalParameter(object AditionalParameter)
        {
            this.AditionalParameter = AditionalParameter;
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

        public Schedule SetTime(int seconds, bool startImmediately = false)
        {
            if (startImmediately)
            {
                this.DateTime = DateTime.Now;
            }
            else
            {
                this.DateTime = DateTime.Now.Add(TimeSpan.FromSeconds(seconds));
            }

            this.FrequencyOfLoop = TimeSpan.FromSeconds(seconds);
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

        public Schedule RunLoop(TaskScheduler taskService)
        {
            this.Loop = true;
            return Run(taskService);
        }

        public bool Validade(out string message)
        {
            message = string.Empty;

            if (this.DateTime == new DateTime())
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
