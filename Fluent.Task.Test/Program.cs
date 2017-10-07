using System;

namespace Fluent.Task.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Now is {DateTime.Now}");

            var taskScheduler = TaskScheduler.Instance().Start();

            Schedule
             .Instance(ShowNow)
             .SetFrequencyTime(10)
             .SetStartImmediately()
             .SetParameter("test parameter")
             .RunLoop(taskScheduler);

            Console.ReadKey();
        }

        private static void ShowNow(object parameter)
        {
            Console.WriteLine($"Now is {DateTime.Now} {parameter}");
        }
    }
}
