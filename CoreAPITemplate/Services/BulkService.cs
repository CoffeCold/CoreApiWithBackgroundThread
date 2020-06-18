using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using System.Threading;
using CoreAPI.Models;

namespace CoreAPI.Services
{


    public class BulkService : IBulkService, IDisposable
    {

        private readonly ILogger<BulkService> _logger;
        private readonly IJobManagementService _jobManagementService;

        public BulkService(ILogger<BulkService> logger, IJobManagementService jobManagementService)
        {
            _logger = logger;
            _jobManagementService = jobManagementService;

        }




        public async Task ProcessBulk(Job bulkjob)
        {
            _logger.LogInformation("bulkjob {0} called", bulkjob.JobId);
            bulkjob.StartDate = DateTime.Now;
            bulkjob.JobState = JobState.Started;
            if (await _jobManagementService.UpdateJob(bulkjob) == 0) return;
            if (await _jobManagementService.EnterLog(bulkjob.JobId, String.Format("bulkjob {0} started", bulkjob.JobId)) == 0) return;

            _logger.LogInformation("bulkjob {0} updated", bulkjob.JobId);
            // do work
            Thread.Sleep(5000);
            _logger.LogInformation("bulkjob {0} ended", bulkjob.JobId);

            bulkjob.StopDate = DateTime.Now;
            bulkjob.JobState = JobState.Ended;
            await _jobManagementService.UpdateJob(bulkjob);
            await _jobManagementService.EnterLog(bulkjob.JobId, String.Format("bulkjob {0} stopped", bulkjob.JobId));
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
        // ~BulkServiceService()
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
