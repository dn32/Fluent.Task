using System;
using System.Threading;

namespace Fluent.Task.Test
{
    public class Program
    {
        const int PROCESSES_COUNT = 10000;
        static int previousSecond = 0;

        private static void Operation(Schedule param)
        {
            var task = param as Schedule;
            var delay = DateTime.Now.Subtract(task.DateTime).TotalSeconds;
            if (DateTime.Now.Second != previousSecond)
            {
                previousSecond = DateTime.Now.Second;
                Console.WriteLine($"Delay {delay} {task.DateTime}");
            }
        }

        static void Main(string[] args)
        {
            var rnd = new Random();

            var takService = TaskScheduler.Instance().Start();

            for (int i = 0; i < PROCESSES_COUNT; i++)
            {
                int seconds = rnd.Next(60, 1200);

                Schedule
               .Instance()
               .SetAction(Operation)
               .SetName($"task-{Guid.NewGuid()}")
               .SetTime(seconds)
               .SetLoop()
               .Run(takService);

                if ((i + 1) % (PROCESSES_COUNT / 10) == 0)
                {
                    Console.WriteLine((i + 1) + " added");
                }
            }

            while (true)
            {
                Console.WriteLine(takService.RunninCount() + " running");
                Thread.Sleep(1000);
            }
        }
    }
}
