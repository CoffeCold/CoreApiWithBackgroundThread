using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using CoreAPI.Models;
using CoreAPI.Services;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accounts = await _accountService.GetAll();
            if (accounts == null)
            {
                return NotFound();
            }
            return Ok(accounts);
        }

        // GET: api/Accounts/byIban?iban=12AS12432546789&withTransactions=true
        [HttpGet("{byIban}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Account>> GetAccount(string iban, Boolean withTransactions)
        {
            Account account ;
            if (withTransactions)
            {
                account = await _accountService.GetOneByIbanWithTransactions(iban);
                
            }
            else 
            {
                account = await _accountService.GetOneByIban(iban);
            }


            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }


        //PUT: api/accounts/iban?iban=53WE45789432131
        [HttpPut("{iban}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> UpdateAccount(string iban, Account account)
        {
            _logger.LogInformation("Put accounts called for Iban {0}", account.Iban);
            if (iban != account.Iban)
            {
                return BadRequest();
            }
            Account ar = await _accountService.UpdateOne(account);
            if (ar != null)
            {
                return Ok(ar);
            }
            else
            {
                return NotFound();
            }
        }

        //PUT: api/accounts
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Account>> UpdateAccount(Account account)
        {
            _logger.LogInformation("Put accounts called for Iban {0}", account.Iban);
            Account ar = await _accountService.UpdateOne(account);
            if (ar != null)
            {
                return ar;
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/Accounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _logger.LogInformation("Post accounts called for Iban {0}", account.Iban);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Account ar = await _accountService.AddOne(account);
            if (ar != null)
            {
                return Ok(ar);
            }
            return BadRequest();
        }

        // DELETE: api/Accounts/iban?iban=53WE45789432131
        [HttpDelete("{iban}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Account>> DeleteAccount(string iban)
        {
            _logger.LogInformation("Delete accounts called for Iban {0}", iban);

            var account = await _accountService.DeleteOneByIban(iban);
            if (account != null)
            {
                return Ok(account);
            }
            return NotFound();



        }


    }
}
