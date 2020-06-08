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
    public class TransactionsController : Controller
    {
        private ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionService)
        {
            _transactionsService = transactionService;
        }

        // GET: Transactions
        public async Task<ActionResult> Index()
        {
            return View(await _transactionsService.GetAsync());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transactions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}