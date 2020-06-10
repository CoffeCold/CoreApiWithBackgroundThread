using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreAPI.Helpers;
using Microsoft.Extensions.Logging;
using CoreAPI.Models;
using CoreAPI.Services;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkController : ControllerBase
    {
        private readonly ILogger<BulkController> _logger;
        private readonly IBulkService _bulkService;
        private readonly IJobManagementService _jobManagementService;
        public BulkController( ILogger<BulkController> logger, IBulkService bulkService, IJobManagementService jobManagementService)
        {
            _logger = logger;
            _bulkService = bulkService;
            _jobManagementService = jobManagementService;
        }

        // GET: api/Bulk/state/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("state/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobState>> GetState(Guid JobId)
        {
            Job job = await _jobManagementService.GetState(JobId);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        // GET: api/Bulk/logs/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("logs/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobLog>>> GetLogs(Guid id)
        {
            JobQuery jobQuery = new JobQuery() { JobId = id };
            var logs = await _jobManagementService.GetLogs(jobQuery);
            if (logs == null)
            {
                return NotFound();
            }
            return Ok(logs);
        }

        // Post: api/bulk
        // Body : {"JobId":"00000000-0000-0000-0000-000000000000", "description":"abc", "JobProperty1" :"def" ,"ExecutionDomain":"Batch"}
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Job>> Entertask(Job taskset)
        {
            _logger.LogInformation("task entered {0}", taskset.JobId);

            if (!ModelState.IsValid) return BadRequest(ModelState);


            Job jobSettings = await _jobManagementService.ScheduleJob(taskset);
            if (jobSettings != null)
            {
                return Ok(jobSettings);
            }
            return BadRequest();
        }



      }
}
