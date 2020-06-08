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
        private IBulkService _bulkService;

        public BulkController(TasksToRun tasks, ILogger<BulkController> logger, IBulkService bulkService)
        {
            _logger = logger;
            _bulkService = bulkService;
        }

        // GET: api/Bulk/State/2432546789
        [HttpGet("state/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProcessState>> GetState(int Taskid)
        {
            TaskSetting ts = new TaskSetting() { Id = Taskid };
            ProcessState state = await _bulkService.Check(ts);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        // GET: api/Bulk/Logs?Taskid=12432546789
        [HttpGet("logs/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProcessLog>>> GetLogs(int Taskid)
        {
            TaskSetting ts = new TaskSetting() { Id = Taskid };
            var logs = await _bulkService.GetLogs(ts);
            if (logs == null)
            {
                return NotFound();
            }
            return Ok(logs);
        }

        // Post: api/Bulk
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskSetting>> Entertask(TaskSetting taskset)
        {
            _logger.LogInformation("task entered {0}", taskset.Id);

            if (!ModelState.IsValid) return BadRequest(ModelState);


            TaskSetting taskSettings = await _bulkService.StartBulk(taskset);
            if (taskSettings != null)
            {
                return Ok(taskSettings);
            }
            return BadRequest();
        }



      }
}
