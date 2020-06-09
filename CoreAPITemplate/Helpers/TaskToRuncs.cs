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
        private readonly BlockingCollection<JobSettings> _tasks;

        public TasksToRun()
        {
            _tasks = new BlockingCollection<JobSettings>();
        }
        public Guid Enqueue(JobSettings settings)
        {
            _tasks.Add(settings);
            return settings.JobId; 
        }

        public JobSettings Dequeue(CancellationToken token)
        {
            if (_tasks.Any())
            {
                return _tasks.Take(token);
            }
            else 
            {
                return null; 
            }
        }

 
    }
 
    }
