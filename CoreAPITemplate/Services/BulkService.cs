using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{


    public class BulkService : IBulkService, IGenericService, IDisposable
    {

        private TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<BulkService> _logger;
        private readonly TasksToRun _tasks;


        public BulkService(ILogger<BulkService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context, TasksToRun tasks)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
            _tasks = tasks;
        }

        public async Task<IEnumerable<ProcessLog>> GetLogs(TaskSetting taskSettings)
        {
            return await _transactionDBContext.ProcessLogs.Where(p => p.Id == taskSettings.Id).ToListAsync();
        }
        public async Task<TaskSetting> StartBulk(TaskSetting settings)
        {
             _tasks.Enqueue(settings);
            return settings; 

        }
        public async Task<ProcessState> Check(TaskSetting taskSettings)
        {
            ProcessState processtate =  _tasks.GetState(taskSettings);
            return processtate; 
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
