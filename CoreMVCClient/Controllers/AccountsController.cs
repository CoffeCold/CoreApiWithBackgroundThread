using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CoreMVCClient.Services;
using CoreAPI.Models;

namespace CoreMVCClient.Controllers
{
    public class AccountsController : Controller
    {

        private IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        // GET: Accounts
        public async Task<ActionResult> Index()
        {
            return View(await _accountsService.GetAsync());
        }

        // GET: Accounts/Details/5
        public async Task<ActionResult> Details(string iban)
        {
            Account account = await _accountsService.GetAsync(iban);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);

        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            String newName = ((HttpContext.User?.Identity?.Name != null) ? HttpContext.User.Identity.Name : "");
            Account account = new Account() { Name = HttpContext.User.Identity.Name };
            return View(account);

        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Iban,Name,City")] Account account)
        {
            try
            {
                // TODO: Add insert logic here
                Account acc = await _accountsService.AddAsync(account);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounts/Edit/5
        public async Task<ActionResult> Edit(string iban)
        {
            Account account = await _accountsService.GetAsync(iban);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);

        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string iban, [Bind("Iban,Name,City")] Account account)
        {
            try
            {
                if (iban == account.Iban)
                {
                    await _accountsService.EditAsync(account);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounts/Delete/5
        public async Task<ActionResult> Delete(string iban)
        {
            Account account = await this._accountsService.GetAsync(iban);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);

        }

        // POST: Accounts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string iban, [Bind("Iban,Name,City")] Account account)
        {
            try
            {
                // TODO: Add delete logic here

                await _accountsService.DeleteAsync(iban);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}