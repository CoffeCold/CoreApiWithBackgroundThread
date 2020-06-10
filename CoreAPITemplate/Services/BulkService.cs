using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using System.Threading;
using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{


    public class BulkService : IBulkService, IDisposable
    {

        private readonly TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<BulkService> _logger;


        public BulkService(ILogger<BulkService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
        }

 


        public void ProcessBulk(Job taskToRun)
        {
            _logger.LogInformation("ProcessBatch {0} called", taskToRun.JobId);
            Thread.Sleep(5000);
            _logger.LogInformation("ProcessBatch {0} ended", taskToRun.JobId);
            return;
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
