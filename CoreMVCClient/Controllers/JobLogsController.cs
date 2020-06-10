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
    public class JobLogsController : Controller
    {
        private readonly IJobLogsService _JobLogsService;

        public JobLogsController(IJobLogsService JobLogservice)
        {
            _JobLogsService = JobLogservice;
        }

        // GET: JobLogs
        public async Task<ActionResult> Index()
        {
            return View(await _JobLogsService.GetAsync());
        }

        // GET: JobLogs/Details/5
        public ActionResult Details(Guid LogId)
        {
            return View();
        }

        // GET: JobLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobLogs/Create
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

        // GET: JobLogs/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JobLogs/Edit/5
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

        // GET: JobLogs/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JobLogs/Delete/5
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