﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CoreAPI.ConfigurationSettings;
using Microsoft.Extensions.Hosting;
using CoreAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CoreAPI.Services
{


    public class JobBackgroundService : BackgroundService
    {
        private readonly JobsToRunSingleton _jobs;
        private readonly ILogger<JobBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource _tokenSource;

        private Task _currentTask;

        public JobBackgroundService(ILogger<JobBackgroundService> logger, JobsToRunSingleton jobs, IServiceProvider serviceProvider)
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

                    Job taskToRun = _jobs.Dequeue(_tokenSource.Token);
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


        private async Task<Guid> ExecuteJob(Job jobToRun)
        {
            _logger.LogInformation("ExecuteTask {0} called", jobToRun.JobId);
            return await Task.Run(() => JobSelectExcute(jobToRun)).ConfigureAwait(false);
        }

        private async Task<Guid> JobSelectExcute(Job jobToRun)
        {
            _logger.LogInformation("JobSelection & execution {0} called", jobToRun.JobId);
            switch (jobToRun.ExecutionDomain)
            {
                case ExecutionDomain.Batch:
                    {
                        using IServiceScope scope = _serviceProvider.CreateScope();
                        var batchService = scope.ServiceProvider.GetService<IBatchService>();
                        await batchService.ProcessBatch(jobToRun);
                        break;
                    }
                case ExecutionDomain.Bulk:
                    {
                        using IServiceScope scope = _serviceProvider.CreateScope();
                        var bulkService = scope.ServiceProvider.GetService<IBulkService>();
                        await bulkService.ProcessBulk(jobToRun);
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

    public class JobBackgroundService_1 : JobBackgroundService
    {
        public JobBackgroundService_1(ILogger<JobBackgroundService> logger, JobsToRunSingleton jobs, IServiceProvider serviceProvider) : base(logger, jobs, serviceProvider) { }
    }
    public class JobBackgroundService_2 : JobBackgroundService
    {
        public JobBackgroundService_2(ILogger<JobBackgroundService> logger, JobsToRunSingleton jobs, IServiceProvider serviceProvider) : base(logger, jobs, serviceProvider) { }
    }
    public class JobBackgroundService_3 : JobBackgroundService
    {
        public JobBackgroundService_3(ILogger<JobBackgroundService> logger, JobsToRunSingleton jobs, IServiceProvider serviceProvider) : base(logger, jobs, serviceProvider) { }
    }
}
