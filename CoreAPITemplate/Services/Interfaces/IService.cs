using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreAPI.Models;

namespace CoreAPI.Services
{

    public interface IBatchService 
    {
        Task ProcessBatch(Job jobToRun);
    }
    public interface IBulkService 
    {
        Task ProcessBulk(Job jobToRun);


    }
    public interface IJobManagementService
    {
        Task<Job> GetState(Guid id);
        Task<IEnumerable<JobLog>> GetLogs(JobQuery jobQuery);
        Task<Job> ScheduleJob(Job jobSettings);
        Task<IEnumerable<Job>> GetAll();
        Task<int> UpdateJob(Job job);
        Task<int> EnterLog(Guid jobid, String logtext);

    }

}
