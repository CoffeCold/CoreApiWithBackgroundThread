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
    public class BatchController : ControllerBase
    {
        private readonly ILogger<BatchController> _logger;
        private IBatchService _batchService;
        private IJobManagementService _jobAdministrationService; 
        public BatchController( ILogger<BatchController> logger, IBatchService batchService, IJobManagementService jobAdministrationService)
        {
            _logger = logger;
            _batchService = batchService;
            _jobAdministrationService = jobAdministrationService;
        }

        // GET: api/Batch/state/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("state/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobState>> GetState(Guid id)
        {
            JobSettings ts = new JobSettings() { JobId = id };
            JobState state = await _jobAdministrationService.GetState(ts);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        // GET: api/Batch/logs/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("logs/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobLog>>> GetLogs(Guid id)
        {
            JobSettings ts = new JobSettings() { JobId = id };
            var logs = await _jobAdministrationService.GetLogs(ts);
            if (logs == null)
            {
                return NotFound();
            }
            return Ok(logs);
        }

        // Post: api/Batch
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobSettings>> Entertask(JobSettings taskset)
        {
            _logger.LogInformation("task entered {0}", taskset.JobId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (taskset.JobId != new Guid()) return BadRequest(ModelState);

            JobSettings jobSettings = await _jobAdministrationService.ScheduleJob(taskset);
            if (jobSettings != null)
            {
                return Ok(jobSettings);
            }
            return BadRequest();
        }

    }
}
