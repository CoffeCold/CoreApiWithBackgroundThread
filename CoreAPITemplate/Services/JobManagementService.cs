using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAPI.Models;
using CoreAPI.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;


namespace CoreAPI.Services
{
    public class JobManagementService : IJobManagementService
    {
        private TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<JobManagementService> _logger;
        private readonly TasksToRun _tasks;

        public JobManagementService(ILogger<JobManagementService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context, TasksToRun tasks)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
            _tasks = tasks;
        }
        public async Task<IEnumerable<JobLog>> GetLogs(JobSettings jobSettings)
        {
            // stub for time being
            var list = new List<JobLog>();
            list.Add(new JobLog() { LogId = Guid.NewGuid(), Logcomment = "abc", Logdate = DateTime.Now });
            list.Add(new JobLog() { LogId = Guid.NewGuid(), Logcomment = "abc", Logdate = DateTime.Now });
            return await Task.FromResult(list);
            //return await _transactionDBContext.ProcessLogs.Where(p => p.Id == taskSettings.Id).ToListAsync();
        }

        //public async Task<IEnumerable<ProcessLog>> GetLogs(TaskSetting taskSettings)
        //{
        //    return await _transactionDBContext.ProcessLogs.Where(p => p.Id == taskSettings.Id).ToListAsync();
        //}
        public async Task<JobState> GetState(JobSettings jobSettings)
        {
            JobState ps = new JobState() { StateID = jobSettings.JobId, Logdate = DateTime.Now, State = "abc" };
            return await Task.FromResult(ps);
        }

        public async Task<JobSettings> ScheduleJob(JobSettings settings)
        {
            settings.JobId = Guid.NewGuid();
            _tasks.Enqueue(settings);
            _transactionDBContext.JobSettings.Add(settings);
            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                return await Task.FromResult(settings);
            }
            return null;

        }

    }
}
