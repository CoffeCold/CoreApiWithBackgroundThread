using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreAPI.Models;

namespace CoreAPI.Services
{

    public interface IBatchService 
    {
        void ProcessBatch(JobSettings jobToRun);
    }
    public interface IBulkService 
    {
        void ProcessBulk(JobSettings jobToRun);


    }
    public interface IJobManagementService
    {
        Task<JobState> GetState(JobSettings jobSettings);
        Task<IEnumerable<JobLog>> GetLogs(JobSettings jobSettings);
        Task<JobSettings> ScheduleJob(JobSettings jobSettings);


    }

}
