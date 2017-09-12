# Fluent.Task

#### Executing a Looped Activity

```C++

static void Operation(Schedule task)
{
    Console.WriteLine($"Task executed! parameter: {task.AditionalParameter} now is {DateTime.Now}");
}
        
static void Main(string[] args)
{
    var taskScheduler = TaskScheduler.Instance().Start();
    var aditionalParameter = DateTime.Now;

     Schedule
    .Instance(Operation)
    .SetTime(seconds, startImmediately: true)
    .SetAditionalParameter(aditionalParameter)
    .Run(taskScheduler);            
}  
```

```
"Task executed! parameter: 12/09/2017 07:22:03 now is 12/09/2017 07:22:09"
"Task executed! parameter: 12/09/2017 07:22:03 now is 12/09/2017 07:22:11"
"Task executed! parameter: 12/09/2017 07:22:03 now is 12/09/2017 07:22:13"
"Task executed! parameter: 12/09/2017 07:22:03 now is 12/09/2017 07:22:15"
...
```
