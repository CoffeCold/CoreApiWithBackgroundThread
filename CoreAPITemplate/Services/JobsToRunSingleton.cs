using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using CoreAPI.Models;

namespace CoreAPI.Services
{
    public class JobsToRunSingleton
    {
        private readonly BlockingCollection<Job> _tasks;

        public JobsToRunSingleton()
        {
            _tasks = new BlockingCollection<Job>();
        }
        public Guid Enqueue(Job settings)
        {
            _tasks.Add(settings);
            return settings.JobId; 
        }

        public Job Dequeue(CancellationToken token)
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
