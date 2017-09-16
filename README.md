# Fluent.Task

#### Executing a Looped Activity

```C++
using System;

namespace Fluent.Task.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            var taskScheduler = TaskScheduler.Instance().Start();

            Schedule
             .Instance(ShowNow)
             .SetDateTime(DayOfWeek.Monday, hour: 8)
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

```
