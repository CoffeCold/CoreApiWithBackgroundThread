using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CoreAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Services
{
    public class JobManagementService : IJobManagementService
    {
        private readonly JobManagementDBContext _jobmanagementDBContext;
        private readonly ILogger<JobManagementService> _logger;
        private readonly JobsToRunSingleton _jobs;

        public JobManagementService(ILogger<JobManagementService> logger, JobManagementDBContext context, JobsToRunSingleton jobs)
        {
            _logger = logger;
            _jobmanagementDBContext = context;
            _jobs = jobs;
        }
        public async Task<IEnumerable<JobLog>> GetLogs(JobQuery jobSettings)
        {
            _logger.LogInformation("GetLogs {0} called", jobSettings.JobId);
            return await _jobmanagementDBContext.JobLogs.Where(p => p.JobId == jobSettings.JobId).ToListAsync();
        }

        public async Task<IEnumerable<Job>> GetAll()
        {
            _logger.LogInformation("Get scheduled logs called");
            return await _jobmanagementDBContext.Jobs.Where(j=>j.JobState == JobState.Scheduled).ToListAsync();
        }




        public async Task<Job> GetState(Guid jobId)
        {
            _logger.LogInformation("GetState {0} called", jobId);
            return await _jobmanagementDBContext.Jobs.Where(p => p.JobId == jobId).SingleOrDefaultAsync();
        }

        public async Task<Job> ScheduleJob(Job job)
        {
            _logger.LogInformation("Schedule job started");
            job.JobId = Guid.NewGuid();
            job.ScheduleDate = DateTime.Now;
            job.JobState = JobState.Scheduled;
            _jobmanagementDBContext.Jobs.Add(job);
            if (await _jobmanagementDBContext.SaveChangesAsync() > 0)
            {
                _jobs.Enqueue(job);
                _logger.LogInformation("Schedule job ended");

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
            if (await _jobmanagementDBContext.SaveChangesAsync() > 0)
            {
                return 1;
            }
            return 0; 
        }

        public async Task<int> EnterLog(Guid jobid,  String logtext)
        {
            JobLog joblog = new JobLog() {  JobId = jobid, Logcomment = logtext, Logdate = DateTime.Now, LogId = Guid.NewGuid()};
            _jobmanagementDBContext.JobLogs.Add(joblog);
            if (await _jobmanagementDBContext.SaveChangesAsync() > 0)
            {
                return 1;
            } 
            return 0;
        }

    }
}
