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
    public class JobsController : Controller
    {

        private IJobsService _JobsService;

        public JobsController(IJobsService JobsService)
        {
            _JobsService = JobsService;
        }

        // GET: Jobs
        public async Task<ActionResult> Index()
        {
            return View(await _JobsService.GetAsync());
        }

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(string iban)
        {
            Job Job = await _JobsService.GetAsync(iban);

            if (Job == null)
            {
                return NotFound();
            }

            return View(Job);

        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            Job Job = new Job() { };
            return View(Job);

        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("JobId,Description,ExecutionDomain")] Job Job)
        {
            try
            {
                // TODO: Add insert logic here
                Job acc = await _JobsService.AddAsync(Job);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Edit/5
        public async Task<ActionResult> Edit(string iban)
        {
            Job Job = await _JobsService.GetAsync(iban);

            if (Job == null)
            {
                return NotFound();
            }

            return View(Job);

        }

        // POST: Jobs/Edit/5
        [HttpPost("{jobid:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid jobid, [Bind("JobId,Description,ExecutionDomain")] Job Job)
        {
            try
            {
                if (jobid == Job.JobId)
                {
                    await _JobsService.EditAsync(Job);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Delete/5
        public async Task<ActionResult> Delete(string iban)
        {
            Job Job = await this._JobsService.GetAsync(iban);

            if (Job == null)
            {
                return NotFound();
            }

            return View(Job);

        }

        // POST: Jobs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string iban, [Bind("JobId,Description,ExecutionDomain")] Job Job)
        {
            try
            {
                // TODO: Add delete logic here

                await _JobsService.DeleteAsync(iban);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}