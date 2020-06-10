using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Xunit;

using CoreAPI.Controllers;
using CoreAPI.Services;
using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreXUnitTest
{
    public class UnitTestBatchController
    {
        private ILogger<BatchController> _BatchControllerLogger;
        private ILogger<JobManagementService> _JobManagementServiceLogger;
        private IJobManagementService _JobManagementService;
        private IOptions<AppSettings> _appSettings;
        private TransactionDBContext _transactionDBContext;

 

        [Fact(DisplayName = "BatchController POST: api/Job Job inserted ")]
        public async Task Create_ReturnsNewlyCreatedJobLogs()
        {
            // Arrange
            GetInjections();
            var batchcontroller = new BatchController(_BatchControllerLogger, _JobManagementService);

            // Act
            var actionresult = await batchcontroller.Entertask(new Job() {  Description = "foo", JobProperty1 = "bar"});


            // Assert
            OkObjectResult oresult = Assert.IsType<OkObjectResult>(actionresult.Result);
            JobLog trans = Assert.IsType<JobLog>(oresult.Value);
            Assert.NotEqual(Guid.Empty, trans.JobId);
        }


        #region injections
        private void GetInjections()
        {
            _BatchControllerLogger = (_BatchControllerLogger is null) ? GetBatchControllerLogger() : _BatchControllerLogger;
            _JobManagementServiceLogger = (_JobManagementServiceLogger is null) ? GetJobManagementServiceLogger() : _JobManagementServiceLogger;
            _transactionDBContext = (_transactionDBContext is null) ? GetContext() : _transactionDBContext;
            _appSettings = (_appSettings is null) ? GetAppSettings() : _appSettings;
            _JobManagementService = (_JobManagementService is null) ? GetJobLogservice(_JobManagementServiceLogger, _appSettings, _transactionDBContext) : _JobManagementService;
        }

        private ILogger<JobManagementService> GetJobManagementServiceLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<JobManagementService> logger = new Logger<JobManagementService>(loggerFactory);
            return logger;
        }

        private TransactionDBContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TransactionDBContext>()
             .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransactionDB;ConnectRetryCount=0").Options;
            var context = new TransactionDBContext(options);
            return context;
        }
        private ILogger<BatchController> GetBatchControllerLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<BatchController> logger = new Logger<BatchController>(loggerFactory);
            return logger;
        }
        private IJobManagementService GetJobLogservice(ILogger<JobManagementService> logger, IOptions<AppSettings> appsettings, TransactionDBContext context)
        {
            JobsToRun jtr = new JobsToRun();
            return new JobManagementService(logger, context,jtr);
        }

        private static IOptions<AppSettings> GetAppSettings()
        {
            return Options.Create<AppSettings>(new AppSettings() { Foo = "", Bar = "" });
        }

        #endregion

    }
}
