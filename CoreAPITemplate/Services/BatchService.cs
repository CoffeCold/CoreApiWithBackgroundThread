﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Diagnostics; 
using CoreAPI.Models;

namespace CoreAPI.Services
{


    public class BatchService : IBatchService, IDisposable
    {

        private readonly ILogger<BatchService> _logger;
        private readonly IJobManagementService _jobManagementService;

        public BatchService(ILogger<BatchService> logger, IJobManagementService jobManagementService )
        {
            _logger = logger;
            _jobManagementService = jobManagementService;
        }
        





        public async Task ProcessBatch(Job batchjob)
        {
            _logger.LogInformation("batchjob {0} called", batchjob.JobId);
            batchjob.StartDate = DateTime.Now;
            batchjob.JobState = JobState.Started;
            if (await _jobManagementService.UpdateJob(batchjob) == 0) return;
            if (await _jobManagementService.EnterLog(batchjob.JobId,  String.Format("batchjob {0} started", batchjob.JobId)) == 0) return;

            _logger.LogInformation("batchjob {0} updated", batchjob.JobId);
            // do work
            _logger.LogInformation("batchjob {0} phase1", batchjob.JobId);
            ConsumeCPU(20, 5);

            await _jobManagementService.EnterLog(batchjob.JobId, String.Format("batchjob {0} phase2", batchjob.JobId));
            // do work
            _logger.LogInformation("batchjob {0} phase2", batchjob.JobId);
            ConsumeCPU(80, 5);
            await _jobManagementService.EnterLog(batchjob.JobId, String.Format("batchjob {0} phase 3", batchjob.JobId));
            // do work
            _logger.LogInformation("batchjob {0} phase3", batchjob.JobId);
            ConsumeCPU(20, 5);
            await _jobManagementService.EnterLog(batchjob.JobId, String.Format("batchjob {0} phase 4", batchjob.JobId));
            // do work
            _logger.LogInformation("batchjob {0} phase4", batchjob.JobId);
            Thread.Sleep(5000);

            _logger.LogInformation("batchjob {0} phase5", batchjob.JobId);
            ConsumeCPU(20, 5);

            _logger.LogInformation("batchjob {0} phase6", batchjob.JobId);
            ConsumeCPU(40, 5);

            _logger.LogInformation("batchjob {0} phase7", batchjob.JobId);
            ConsumeCPU(80, 5);

            _logger.LogInformation("batchjob {0} phase8", batchjob.JobId);
            ConsumeCPU(20, 5);

            _logger.LogInformation("batchjob {0} ended", batchjob.JobId);

            batchjob.StopDate = DateTime.Now;
            batchjob.JobState = JobState.Ended;
            await _jobManagementService.UpdateJob(batchjob);
            await _jobManagementService.EnterLog(batchjob.JobId, String.Format("batchjob {0} stopped", batchjob.JobId));

        }

        public static void ConsumeCPU(int percentage, int seconds)
        {
            int starts = 0; 
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("percentage");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            starts++;
            while (true)
            {
                // Make the loop go on for "percentage" milliseconds then sleep the 
                // remaining percentage milliseconds. So 40% utilization means work 40ms and sleep 60ms
                if (watch.ElapsedMilliseconds > percentage)
                {
                    Thread.Sleep(100 - percentage);
                    watch.Reset();
                    starts++;
                    if (starts == seconds * 10) return; 
                    watch.Start();
                }
            }
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BatchServiceService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


  




    }
}
