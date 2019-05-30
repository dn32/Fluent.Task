﻿using FluentTask.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentTask
{
    public class TaskScheduler
    {
        private List<Schedule> TaskList { get; set; }
        private Thread Process { get; set; }
        private bool FinalizeProcess { get; set; }
        private CancellationToken CancellationToken { get; set; }

        public static TaskScheduler Instance()
        {
            return new TaskScheduler();
        }

        private TaskScheduler()
        {
            var cts = new CancellationTokenSource();
            CancellationToken = cts.Token;
            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) => { cts.Cancel(); };

            TaskList = new List<Schedule>();
        }

        public TaskScheduler Add(Schedule task)
        {
            ValidadeAdd(task);

            lock (TaskList)
            {
                task.State = eStateOfTask.WAITING;
                TaskList.Add(task);
            }

            return this;
        }

        public Schedule Get(string name)
        {
            lock (TaskList)
            {
                return TaskList.FirstOrDefault(x => x.Name == name);
            }
        }

        private Schedule GetFirstOrDefault()
        {
            lock (TaskList)
            {
                return TaskList.Where(x => x.State == eStateOfTask.WAITING).OrderBy(x => x.LoopSettings.DateTime).FirstOrDefault();
            }
        }

        private List<Schedule> GetByTime(DateTime dateTime)
        {
            lock (TaskList)
            {
                return TaskList
                       .OrderBy(x => x.LoopSettings.DateTime)
                       .Where(x => x.State == eStateOfTask.WAITING && x.LoopSettings.DateTime <= dateTime)
                       .ToList();
            }
        }

        public int Remove(string name)
        {
            lock (TaskList)
            {
                return TaskList.RemoveAll(x => x.Name == name);
            }
        }

        public int Count()
        {
            lock (TaskList)
            {
                return TaskList.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clock">Clock in milliseconds. Minimum value 10ms. Default 100ms.</param>
        /// <returns></returns>
        public TaskScheduler Start(int clock = 100)
        {
            if (FinalizeProcess)
            {
                throw new Exception("The process is finished.");
            }

            if (clock < 10)
            {
                clock = 10;
            }

            if (Process != null)
            {
                return this;
            }

            Process = new Thread(() => ProcessRuning(clock));
            Process.Start();

            return this;
        }

        private void ProcessRuning(int clock)
        {
            while (true)
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (FinalizeProcess)
                {
                    return;
                }

                var tasks = GetByTime(DateTime.Now);
                if (tasks.Count == 0)
                {
                    Task.Delay(clock, CancellationToken).Wait();
                }

                Parallel.ForEach(tasks, task =>
                {
                    if (FinalizeProcess)
                    {
                        return;
                    }

                    task.State = eStateOfTask.PROCESSING;
                    Task.Factory.StartNew(() => RunTask(task));
                });
            }
        }

        public int RunninCount()
        {
            return TaskList.Count(x => x.State == eStateOfTask.RUNNING);
        }

        private void RunTask(Schedule task)
        {
            task.State = eStateOfTask.RUNNING;
            task.Action(task.Parameter);
            if (task.LoopSettings.IsLoop)
            {
                if (task.LoopSettings.FrequencyType != eFrequencyType.BY_INTERVAL && !task.LoopSettings.StartImmediately)
                {
                    Task.Delay(1000, CancellationToken).Wait(); //You must wait at least 1 second to not schedule the task for the time you have just run.
                }

                task.Restart();
            }
            else
            {
                task.State = eStateOfTask.FINALIZED;
                Remove(task.Name);
            }
        }

        public TaskScheduler Stop()
        {
            lock (TaskList)
            {
                TaskList.Clear();
            }

            FinalizeProcess = true;
            return this;
        }

        #region PRIVATES

        private void ValidadeAdd(Schedule task)
        {
            if (!task.Validade(out string message))
            {
                throw new Exception(message);
            }

            if (Get(task.Name) != null)
            {
                throw new DuplicateWaitObjectException(task.Name, $"A task with this name already exists {task.Name}");
            }
        }

        #endregion
    }
}
