# Fluent.Task

#### Executing a Looped Activity

```C#

static void Operation(Schedule task)
{
    Console.WriteLine("Task executed!");
}
        
static void Main(string[] args)
{
    var takService = TaskScheduler.Instance().Start();
    
    Schedule
   .Instance(Operation)
   .SetTime(2)
   .RunLoop(takService);              
}  
```

```
"Task executed!"
"Task executed!"
"Task executed!"
"Task executed!"
"Task executed!"
...
```
