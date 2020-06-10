using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreAPI.Models;
using CoreAPI.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Services
{
    public class JobManagementService : IJobManagementService
    {
        private readonly TransactionDBContext _transactionDBContext;
        private readonly ILogger<JobManagementService> _logger;
        private readonly JobsToRun _jobs;

        public JobManagementService(ILogger<JobManagementService> logger, TransactionDBContext context, JobsToRun jobs)
        {
            _logger = logger;
            _transactionDBContext = context;
            _jobs = jobs;
        }
        public async Task<IEnumerable<JobLog>> GetLogs(JobQuery jobSettings)
        {
            _logger.LogInformation("GetLogs {0} called", jobSettings.JobId);
            return await _transactionDBContext.JobLogs.Where(p => p.JobId == jobSettings.JobId).ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetAll()
        {
            _logger.LogInformation("Get scheduled logs called");
            return await _transactionDBContext.Jobs.Where(j=>j.JobState == JobState.Scheduled).ToListAsync();
        }




        public async Task<Job> GetState(Guid jobId)
        {
            _logger.LogInformation("GetState {0} called", jobId);
            return await _transactionDBContext.Jobs.Where(p => p.JobId == jobId).SingleOrDefaultAsync();
        }

        public async Task<Job> ScheduleJob(Job job)
        {
            job.JobId = Guid.NewGuid();
            job.ScheduleDate = DateTime.Now;
            job.JobState = JobState.Scheduled;
            _transactionDBContext.Jobs.Add(job);
            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                _jobs.Enqueue(job);
                return await Task.FromResult(job);
            }
            return null;
        }

        public async Task<int> UpdateJob(Job job)
        {
            Job job_toupdate = await GetState(job.JobId);
            if (job_toupdate != null)
            {
                job_toupdate.StartDate = job.StartDate;
                job_toupdate.StopDate = job.StopDate;
                job_toupdate.JobState= job.JobState;
            }
            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                return 1;
            }
            return 0; 
        }

        public async Task<int> EnterLog(Guid jobid,  String logtext)
        {
            JobLog joblog = new JobLog() {  JobId = jobid, Logcomment = logtext, Logdate = DateTime.Now, LogId = Guid.NewGuid()};
            _transactionDBContext.JobLogs.Add(joblog);
            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                return 1;
            } 
            return 0;
        }

    }
}
