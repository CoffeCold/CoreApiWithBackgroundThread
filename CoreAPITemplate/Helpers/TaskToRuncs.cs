using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using CoreAPI.Models;

namespace CoreAPI.Helpers
{
    public class TasksToRun
    {
        private readonly BlockingCollection<TaskSetting> _tasks;

        public TasksToRun() => _tasks = new BlockingCollection<TaskSetting>();

        public int Enqueue(TaskSetting settings)
        {
            _tasks.Add(settings);
            return settings.Id; 
        }

        public TaskSetting Dequeue(CancellationToken token) => _tasks.Take(token);

        internal ProcessState GetState(TaskSetting taskSettings)
        {
            //todo
            return new ProcessState() {  Id= taskSettings.Id , Logdate= DateTime.Now, State = "abc"};
        }
    }
 
    }
