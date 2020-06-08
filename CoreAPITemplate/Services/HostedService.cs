using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CoreAPI.Helpers;
using Microsoft.Extensions.Hosting;
using CoreAPI.Models;
using Microsoft.Extensions.Logging;

namespace CoreAPI.Services
{

  

    public class MyBackgroundService : BackgroundService
    {
        private readonly TasksToRun _tasks;
        private readonly ILogger<BatchService> _logger;

        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public MyBackgroundService(ILogger<BatchService> logger,TasksToRun tasks)
        {
            _tasks = tasks;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken).ConfigureAwait(false);

                    TaskSetting taskToRun = _tasks.Dequeue(_tokenSource.Token);
                    if (taskToRun != null)
                    {
                        //// We need to save executable task, 
                        //// so we can gratefully wait for it's completion in Stop method
                        _currentTask = ExecuteTask(taskToRun);
                        await _currentTask;
                    }
                }
                catch (OperationCanceledException)
                {
                    // execution cancelled
                }
            }

        }
        //public async Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        //    while (cancellationToken.IsCancellationRequested == false)
        //    {
        //        try
        //        {
        //            TaskSetting taskToRun = _tasks.Dequeue(_tokenSource.Token);

        //            //// We need to save executable task, 
        //            //// so we can gratefully wait for it's completion in Stop method
        //            _currentTask = ExecuteTask(taskToRun);
        //            await _currentTask;
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            // execution cancelled
        //        }
        //    }
        //}

        private async Task<int> ExecuteTask(TaskSetting taskToRun)
        {
            _logger.LogInformation("ExecuteTask {0} called", taskToRun.Id);
            return await Task.Run(() => SomeMethodAsync(taskToRun)).ConfigureAwait(false);
        }

        private int SomeMethodAsync(TaskSetting taskToRun)
        {
            _logger.LogInformation("SomeMethodAsync {0} called", taskToRun.Id);
            System.Threading.Thread.Sleep(5000);
            _logger.LogInformation("SomeMethodAsync {0} ended", taskToRun.Id);
            return taskToRun.Id; 

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
