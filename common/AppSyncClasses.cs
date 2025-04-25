using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading;
using ModAuth;
using System.Threading.Tasks;

public class AppSyncClasses
{
    public static void StartAll()
    {
        try
        {
            DeleteTmpFiles_timer = new System.Timers.Timer();
            DeleteTmpFiles_timer.Interval = (1000 * 60 * 15); // first after 15 mins
            DeleteTmpFiles_timer.Elapsed += new System.Timers.ElapsedEventHandler(DeleteTmpFiles_timerElapsed);
            DeleteTmpFiles_timer.Start();
        }
        catch (Exception exc)
        {
            ErrorLog.addLog("", "AppSyncClasses.StartAll.DeleteTmpFiles_timer", exc.ToString());
        }

        try
        {
            if (CommonUtilities.getSYS_SETTING("is_clear_log") == "true" || CommonUtilities.getSYS_SETTING("is_clear_log") == "1")
            {
                ClearErrorLog_timer = new System.Timers.Timer();
                ClearErrorLog_timer.Interval = (1000 * 60 * 20); // first after 20 mins
                ClearErrorLog_timer.Elapsed += new System.Timers.ElapsedEventHandler(ClearErrorLog_timerElapsed);
                ClearErrorLog_timer.Start();
            }
        }
        catch (Exception exc)
        {
            ErrorLog.addLog("", "AppSyncClasses.StartAll.ClearErrorLog_timer", exc.ToString());
        }
    }

    private static System.Timers.Timer DeleteTmpFiles_timer;
    private static void DeleteTmpFiles_timerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK

        AppSyncClasses.DeleteTmpFiles_start(); // only once
    }
    private static void DeleteTmpFiles_process()
    {
        try
        {
            if (!Directory.Exists(Path.Combine(App.SRP, "files"))) return;
            if (!Directory.Exists(Path.Combine(App.SRP, "files/tmp"))) return;
            DirectoryInfo TmpFilesInfo = new DirectoryInfo(Path.Combine(App.SRP, "files/tmp"));
            foreach (FileInfo file in TmpFilesInfo.GetFiles())
            {
                if (file.LastAccessTime < DateTime.Now.AddDays(-7))
                    file.Delete();
            }
            foreach (DirectoryInfo dir in TmpFilesInfo.GetDirectories())
            {
                if (dir.LastAccessTime < DateTime.Now.AddDays(-7))
                    dir.Delete(true);
            }
        }
        catch (Exception exc)
        {
            ErrorLog.addLog("", "AppSyncClasses.DeleteTmpFiles_start", exc.ToString());
        }
    }
    private static void DeleteTmpFiles_start()
    {
        //Action<object> action = (object obj) => { DeleteTmpFiles_process(); };
        //AppUtilsTaskScheduler.AddTask(action, "DeleteTmpFiles_start");

        //ThreadStart start = new ThreadStart(Thread_fillRewriteTool);
        //Thread t = new Thread(start);
        //t.Priority = ThreadPriority.Lowest;
        //t.Start();
        ThreadStart start = new ThreadStart(DeleteTmpFiles_process);
        Thread t = new Thread(start);
        t.Priority = ThreadPriority.Lowest;
        t.Start();
    }

    private static System.Timers.Timer ClearErrorLog_timer;
    private static void ClearErrorLog_timerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // background OK
        AppSyncClasses.ClearErrorLog_start(); // only once
    }
    private static void ClearErrorLog_process()
    {
        try
        {
            using (DCmodAuth dc = new DCmodAuth())
            {
                dc.Delete(dc.dbAuthErrorLOGs.Where(x => x.logDateTime <= DateTime.Now.AddDays(-2)));
                dc.SaveChanges();
            }
        }
        catch (Exception exc)
        {
            ErrorLog.addLog("", "AppSyncClasses.ClearErrorLog_process", exc.ToString());
        }
    }
    private static void ClearErrorLog_start()
    {
        //Action<object> action = (object obj) => { ClearErrorLog_process(); };
        //AppUtilsTaskScheduler.AddTask(action, "ClearErrorLog_start");
        ThreadStart start = new ThreadStart(ClearErrorLog_process);
        Thread t = new Thread(start);
        t.Priority = ThreadPriority.Lowest;
        t.Start();
    }
}
class xxx
{

    private static void xxx_process()
    {
        try
        {
        }
        catch (Exception exc)
        {
            ErrorLog.addLog("", "AppSyncClasses.xxx_process", exc.ToString());
        }
    }
    private static void xxx_start()
    {
        //Action<object> action = (object obj) => { xxx_process(); };
        //AppUtilsTaskScheduler.AddTask(action, "xxx_start");
        ThreadStart start = new ThreadStart(xxx_process);
        Thread t = new Thread(start);
        t.Priority = ThreadPriority.Lowest;
        t.Start();
    }
}

public class AppUtilsTaskScheduler
{
    public static int MaxDegreeOfParallelism = 1;
    public class TaskListItem
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TaskListItem(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
    }
    public static List<TaskListItem> TaskList = new List<TaskListItem>();
    public static LimitedConcurrencyLevelTaskScheduler scheduler;
    public static int TasksCount()
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        return scheduler.TasksCount;
    }
    public static List<TaskListItem> TasksQueue()
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        return scheduler.TasksList.ToList();
    }
    public static void AddTask(Action<object> action, object taskName)
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        NamedTask t = new NamedTask(action, taskName);
        t.Start(scheduler);
        TaskList.Add(new TaskListItem(taskName + "", DateTime.Now));
    }
    class NamedTask : Task
    {
        public string TaskName { get; set; }
        public DateTime Date { get; set; }
        public NamedTask(Action<object> action, object taskName)
            : base(action, taskName)
        {
            TaskName = taskName + "";
            Date = DateTime.Now;
        }
    }
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        // The list of tasks to be executed  
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        public int TasksCount
        {
            get { return _tasks.Count; }
        }
        public List<TaskListItem> TasksList
        {
            get
            {
                var tmpList = _tasks.ToList();
                return tmpList.Where(x => ((NamedTask)x) != null).Select(x => ((NamedTask)x)).Select(x => new TaskListItem(x.TaskName, x.Date)).ToList();
            }
        }
        // The maximum concurrency level allowed by this scheduler.  
        private readonly int _maxDegreeOfParallelism;

        // Indicates whether the scheduler is currently processing work items.  
        private int _delegatesQueuedOrRunning = 0;

        // Creates a new instance with the specified degree of parallelism.  
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        // Queues a task to the scheduler.  
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough  
            // delegates currently queued or running to process tasks, schedule another.  
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        // Inform the ThreadPool that there's work to be executed for this scheduler.  
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue 
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread 
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        // Attempts to execute the specified task on the current thread.  
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue 
            if (taskWasPreviouslyQueued)
                // Try to run the task.  
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            else
                return base.TryExecuteTask(task);
        }

        // Attempt to remove a previously scheduled task from the scheduler.  
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        // Gets the maximum concurrency level supported by this scheduler.  
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        // Gets an enumerable of the tasks currently scheduled on this scheduler.  
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}
public class AppUtilsTaskSchedulerPriority
{
    public static int MaxDegreeOfParallelism = 1;
    public class TaskListItem
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public TaskListItem(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
    }
    public static List<TaskListItem> TaskList = new List<TaskListItem>();
    public static LimitedConcurrencyLevelTaskScheduler scheduler;
    public static int TasksCount()
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        return scheduler.TasksCount;
    }
    public static List<TaskListItem> TasksQueue()
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        return scheduler.TasksList.ToList();
    }
    public static void AddTask(Action<object> action, object taskName)
    {
        if (scheduler == null) scheduler = new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism);
        NamedTask t = new NamedTask(action, taskName);
        t.Start(scheduler);
        TaskList.Add(new TaskListItem(taskName + "", DateTime.Now));
    }
    class NamedTask : Task
    {
        public string TaskName { get; set; }
        public DateTime Date { get; set; }
        public NamedTask(Action<object> action, object taskName)
            : base(action, taskName)
        {
            TaskName = taskName + "";
            Date = DateTime.Now;
        }
    }
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        // The list of tasks to be executed  
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        public int TasksCount
        {
            get { return _tasks.Count; }
        }
        public List<TaskListItem> TasksList
        {
            get
            {
                var tmpList = _tasks.ToList();
                return tmpList.Where(x => ((NamedTask)x) != null).Select(x => ((NamedTask)x)).Select(x => new TaskListItem(x.TaskName, x.Date)).ToList();
            }
        }
        // The maximum concurrency level allowed by this scheduler.  
        private readonly int _maxDegreeOfParallelism;

        // Indicates whether the scheduler is currently processing work items.  
        private int _delegatesQueuedOrRunning = 0;

        // Creates a new instance with the specified degree of parallelism.  
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        // Queues a task to the scheduler.  
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough  
            // delegates currently queued or running to process tasks, schedule another.  
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        // Inform the ThreadPool that there's work to be executed for this scheduler.  
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue 
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread 
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        // Attempts to execute the specified task on the current thread.  
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue 
            if (taskWasPreviouslyQueued)
                // Try to run the task.  
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            else
                return base.TryExecuteTask(task);
        }

        // Attempt to remove a previously scheduled task from the scheduler.  
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        // Gets the maximum concurrency level supported by this scheduler.  
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        // Gets an enumerable of the tasks currently scheduled on this scheduler.  
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}
