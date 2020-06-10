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
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;
        private readonly IJobManagementService _jobManagementService; 
        public JobController( ILogger<JobController> logger, IJobManagementService jobAdministrationService)
        {
            _logger = logger;
            _jobManagementService = jobAdministrationService;
        }

        // GET: api/Job
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Job>>> GetAll()
        {
            IEnumerable<Job> jobs = await _jobManagementService.GetAll();
            if (jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }

        // GET: api/Job/state/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("state/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Job>> GetState(Guid id)
        {
            Job job = await _jobManagementService.GetState(id);
            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }

        // GET: api/Job/logs/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("logs/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobLog>>> GetLogs(Guid id)
        {
            //TODO extend with other properties
            JobQuery jobQuery = new JobQuery() { JobId = id };
            var logs = await _jobManagementService.GetLogs(jobQuery);
            if (logs == null)
            {
                return NotFound();
            }
            return Ok(logs);
        }

        // Post: api/Job
        // Body : {"JobId":"00000000-0000-0000-0000-000000000000", "description":"abc", "JobProperty1" :"def" ,"ExecutionDomain":"Job"}
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Job>> Entertask(Job taskset)
        {
            _logger.LogInformation("task entered {0}", taskset.JobId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (taskset.JobId != new Guid()) return BadRequest(ModelState);

            Job jobSettings = await _jobManagementService.ScheduleJob(taskset);
            if (jobSettings != null)
            {
                return Ok(jobSettings);
            }
            return BadRequest();
        }

    }
}
