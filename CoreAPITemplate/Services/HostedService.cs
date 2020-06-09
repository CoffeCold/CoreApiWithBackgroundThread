using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CoreAPI.Helpers;
using Microsoft.Extensions.Hosting;
using CoreAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CoreAPI.Services
{

  

    public class MyBackgroundService : BackgroundService
    {
        private readonly TasksToRun _tasks;
        private readonly ILogger<BatchService> _logger;
        private IServiceProvider _serviceProvider; 
        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public MyBackgroundService(ILogger<BatchService> logger, TasksToRun tasks, IServiceProvider serviceProvider)
        {
            _tasks = tasks;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken).ConfigureAwait(false);

                    JobSettings taskToRun = _tasks.Dequeue(_tokenSource.Token);
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


        private async Task<Guid> ExecuteTask(JobSettings taskToRun)
        {
            _logger.LogInformation("ExecuteTask {0} called", taskToRun.JobId);
            return await Task.Run(() => SomeMethodAsync(taskToRun)).ConfigureAwait(false);
        }

        private Guid SomeMethodAsync(JobSettings taskToRun)
        {
            _logger.LogInformation("SomeMethodAsync {0} called", taskToRun.JobId);
            using (var scope = _serviceProvider.CreateScope())
            {
                var xService = scope.ServiceProvider.GetService<IBatchService>();
                xService.ProcessBatch(taskToRun);
            }
            _logger.LogInformation("SomeMethodAsync {0} ended", taskToRun.JobId);
            return taskToRun.JobId; 

        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel(); // cancel "waiting" for task in blocking collection

            if (_currentTask == null) return;

            // wait when _currentTask is complete
            await Task.WhenAny(_currentTask, Task.Delay(-1, cancellationToken));
        }
    }
}
