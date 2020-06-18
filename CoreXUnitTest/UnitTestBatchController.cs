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
using CoreAPI.ConfigurationSettings;

namespace CoreXUnitTest
{
    public class UnitTestBatchController
    {
        private ILogger<JobController> _JobControllerLogger;
        private ILogger<JobManagementService> _JobManagementServiceLogger;
        private IJobManagementService _JobManagementService;
        private IOptions<AppSettings> _appSettings;
        private JobManagementDBContext _jobmanagemenDBContext;

 

        [Fact(DisplayName = "BatchController POST: api/Job Job inserted ")]
        public async Task Create_ReturnsNewlyCreatedJobLogs()
        {
            // Arrange
            GetInjections();
            var batchcontroller = new JobController(_JobControllerLogger, _JobManagementService);

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
            _JobControllerLogger = (_JobControllerLogger is null) ? GetJobControllerLogger() : _JobControllerLogger;
            _JobManagementServiceLogger = (_JobManagementServiceLogger is null) ? GetJobManagementServiceLogger() : _JobManagementServiceLogger;
            _jobmanagemenDBContext = (_jobmanagemenDBContext is null) ? GetContext() : _jobmanagemenDBContext;
            _appSettings = (_appSettings is null) ? GetAppSettings() : _appSettings;
            _JobManagementService = (_JobManagementService is null) ? GetJobLogservice(_JobManagementServiceLogger, _appSettings, _jobmanagemenDBContext) : _JobManagementService;
        }

        private ILogger<JobManagementService> GetJobManagementServiceLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<JobManagementService> logger = new Logger<JobManagementService>(loggerFactory);
            return logger;
        }

        private JobManagementDBContext GetContext()
        {
            var options = new DbContextOptionsBuilder<JobManagementDBContext>()
             .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransactionDB;ConnectRetryCount=0").Options;
            var context = new JobManagementDBContext(options);
            return context;
        }
        private ILogger<JobController> GetJobControllerLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<JobController> logger = new Logger<JobController>(loggerFactory);
            return logger;
        }
        private IJobManagementService GetJobLogservice(ILogger<JobManagementService> logger, IOptions<AppSettings> appsettings, JobManagementDBContext context)
        {
            JobsToRunSingleton jtr = new JobsToRunSingleton();
            return new JobManagementService(logger, context,jtr);
        }

        private static IOptions<AppSettings> GetAppSettings()
        {
            return Options.Create<AppSettings>(new AppSettings() { Foo = "", Bar = "" });
        }

        #endregion

    }
}
