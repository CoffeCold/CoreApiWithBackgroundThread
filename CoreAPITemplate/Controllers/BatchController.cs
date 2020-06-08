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

        public BatchController(TasksToRun tasks, ILogger<BatchController> logger, IBatchService batchService, TasksToRun task)
        {
            _logger = logger;
            _batchService = batchService;
        }

        // GET: api/Batch/state/123
        [HttpGet("state/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProcessState>> GetState(int id)
        {
            TaskSetting ts = new TaskSetting() { Id = id };
            ProcessState state = await _batchService.Check(ts);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        // GET: api/Batch/logs/12
        [HttpGet("logs/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcessLog>>> GetLogs(int id)
        {
            TaskSetting ts = new TaskSetting() { Id = id };
            var logs = await _batchService.GetLogs(ts);
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
        public async Task<ActionResult<TaskSetting>> Entertask(TaskSetting taskset)
        {
            _logger.LogInformation("task entered {0}", taskset.Id);

            if (!ModelState.IsValid) return BadRequest(ModelState);


            TaskSetting taskSettings = await _batchService.StartBatch(taskset);
            if (taskSettings != null)
            {
                return Ok(taskSettings);
            }
            return BadRequest();
        }

    }
}
