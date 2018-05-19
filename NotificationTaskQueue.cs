namespace Engaze.NotificationManager
{
    using Engaze.NotificationManager.Contract;
    using System;
    using System.Collections;

    public abstract class NotificationTaskQueue : INotificationTaskQueue
    {
         public NotificationContainer UserData { get; private set; }
         

        public TaskDelegate TaskDlgt { get; private set; }
        protected abstract void Task(dynamic data);

        public void Enqueue(dynamic data)
        {
            TaskDlgt = new TaskDelegate(Task);
            
            lock(lockObject)
            {
                if(Busy)
                    _q.Enqueue(TaskDlgt);
                else {
                    Busy = true;
                    TaskDlgt.BeginInvoke(new AsyncCallback(this.TaskCallback), TaskDlgt, data);
                }
            }
        }

        private static Queue _q = new Queue();
        private static bool Busy = false;
        private static object lockObject = new object();
      

        public delegate void TaskDelegate(dynamic data);

        private void TaskCallback(IAsyncResult ar)
        {
            TaskDelegate dlgt = ar.AsyncState as TaskDelegate;
            
            if(dlgt.Equals(TaskDlgt))
                dlgt.EndInvoke(ar);
            NextTask();
        }
        private void NextTask()
        {
            TaskDelegate dlgt;
            lock(lockObject)
            {
                if(_q.Count > 0)
                {
                    dlgt = (TaskDelegate)_q.Dequeue();
                    dlgt.BeginInvoke(TaskCallback,dlgt);
                }
                else
                    Busy = false;
            }
        }

    }
}
