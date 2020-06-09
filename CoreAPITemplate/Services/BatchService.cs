using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;

using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{


    public class BatchService : IBatchService, IDisposable
    {

        private TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<BatchService> _logger;
        private readonly TasksToRun _tasks;


        public BatchService(ILogger<BatchService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context, TasksToRun tasks)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
            _tasks = tasks;
        }

 
        public async Task<JobSettings> StartBatch(JobSettings settings)
        {
             _tasks.Enqueue(settings);
            return await Task.FromResult(settings); 

        }


        public void ProcessBatch(JobSettings taskToRun)
        {
            _logger.LogInformation("ProcessBatch {0} called", taskToRun.JobId);
            Thread.Sleep(5000);
            _logger.LogInformation("ProcessBatch {0} ended", taskToRun.JobId);
            return ;
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
