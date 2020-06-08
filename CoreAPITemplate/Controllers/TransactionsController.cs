using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CoreAPI.Models;
using CoreAPI.Services;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private ITransactionsService _transactionsService;

        public TransactionsController(ILogger<TransactionsController> logger, ITransactionsService transactionsService)
        {
            _logger = logger;
            _transactionsService = transactionsService;
        }

        // GET: api/Transactions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var transactions = await _transactionsService.GetAll();
            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        // GET: api/Transactions/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpGet("{guid}")]
        public async Task<ActionResult<Transaction>> GetTransaction(Guid guid)
        {
            var transaction = await _transactionsService.GetOneByGuid(guid);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        // PUT: api/Transactions/69562d2a-6b52-47a4-8089-203efa02a3f0
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutTransaction(Guid guid, Transaction transaction)
        {
            if (guid != transaction.Id)
            {
                return BadRequest();
            }
            var transacn = await _transactionsService.UpdateOne(transaction);
            if (transacn == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(transacn);
            }

        }

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            var transacn = await _transactionsService.AddOne(transaction);

            if (transacn != null)
            {
                return Ok(transacn);
            }
            return BadRequest();
        }

        //POST: api/Transactions/many
        [HttpPost("{many}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> PostTransactions(IEnumerable<Transaction> transactions)
        {
            var transacn = await _transactionsService.AddMany(transactions);

            if (transacn != null)
            {
                return Ok(transacn);
            }
            return BadRequest();
        }

        // DELETE: api/Transactions/69562d2a-6b52-47a4-8089-203efa02a3f0
        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Transaction>> DeleteTransaction(Guid guid)
        {
            var transaction = await _transactionsService.DeleteOneByGuid(guid);

            if (transaction != null)
            {
                return Ok(transaction);
            }
            return BadRequest();
        }

 
    }
}
