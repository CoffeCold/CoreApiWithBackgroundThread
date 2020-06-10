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

  

    public class JobBackgroundService : BackgroundService
    {
        private readonly JobsToRun _jobs;
        private readonly ILogger<BatchService> _logger;
        private IServiceProvider _serviceProvider; 
        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public JobBackgroundService(ILogger<BatchService> logger, JobsToRun jobs, IServiceProvider serviceProvider)
        {
            _jobs = jobs;
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

                    JobSettings taskToRun = _jobs.Dequeue(_tokenSource.Token);
                    if (taskToRun != null)
                    {
                        //// We need to save executable task, 
                        //// so we can gratefully wait for it's completion in Stop method
                        _currentTask = ExecuteJob(taskToRun);
                        await _currentTask;
                    }
                }
                catch (OperationCanceledException)
                {
                    // execution cancelled
                }
            }

        }


        private async Task<Guid> ExecuteJob(JobSettings jobToRun)
        {
            _logger.LogInformation("ExecuteTask {0} called", jobToRun.JobId);
            return await Task.Run(() => JobSelectExcute(jobToRun)).ConfigureAwait(false);
        }

        private Guid JobSelectExcute(JobSettings jobToRun)
        {
            _logger.LogInformation("JobSelection & execution {0} called", jobToRun.JobId);
            switch (jobToRun.ExecutionDomain)
            {
                case ExecutionDomain.Batch:
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var batchService = scope.ServiceProvider.GetService<IBatchService>();
                            batchService.ProcessBatch(jobToRun);
                        }
                        break;
                    }
                case ExecutionDomain.Bulk:
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var bulkService = scope.ServiceProvider.GetService<IBulkService>();
                            bulkService.ProcessBulk(jobToRun);
                        }
                        break;
                    }
            }
 
            _logger.LogInformation("JobSelection & execution job {0} ended", jobToRun.JobId);
            return jobToRun.JobId; 

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
