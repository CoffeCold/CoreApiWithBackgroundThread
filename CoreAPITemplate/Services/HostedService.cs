using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CoreAPI.Helpers;
using Microsoft.Extensions.Hosting;

namespace CoreAPI.Services
{

  

    public class MyBackgroundService : IHostedService
    {
        private readonly TasksToRun _tasks;

        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public MyBackgroundService(TasksToRun tasks) => _tasks = tasks;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    //var taskToRun = _tasks.Dequeue(_tokenSource.Token);

                    //// We need to save executable task, 
                    //// so we can gratefully wait for it's completion in Stop method
                    //_currentTask = ExecuteTask(taskToRun);
                    await _currentTask;
                }
                catch (OperationCanceledException)
                {
                    // execution cancelled
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel(); // cancel "waiting" for task in blocking collection

            if (_currentTask == null) return;

            // wait when _currentTask is complete
            await Task.WhenAny(_currentTask, Task.Delay(-1, cancellationToken));
        }
    }
}
