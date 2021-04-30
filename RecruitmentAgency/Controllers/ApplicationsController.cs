using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;
using RecruitmentAgency.ViewModels;

namespace RecruitmentAgency.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly RecruiterAgencyContext _context;

        public ApplicationsController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Application> recruiterAgencyContext = _context.Applications
                .Include(x => x.Candidate)
                .ThenInclude(x => x.Department)
                .ThenInclude(x => x.City)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.Vacancy)
                .ThenInclude(x => x.JobTitle)
                .Include(x => x.Vacancy.CompanyOffice.Company)
                .Include(x => x.Vacancy.Customer)
                .Where(x => x.Vacancy.RecruiterId == int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.Candidate.FirstName.Contains(searchQuery)
            //        || x.Candidate.LastName.Contains(searchQuery)
            //        || x.Vacancy.JobTitle.Name.Contains(searchQuery));
            //}

            //var paginated = await Paginated<Application>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;
            var response = new VacancyApplicationsResponse
            {
                VacancyId = null,
                Applications = await recruiterAgencyContext.ToListAsync()
            };

            return View(response);
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Candidate)
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public async Task<IActionResult> Create(int vacancyId)
        {
            ViewData["CandidateId"] = await _context.Candidates
                .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.CandidateId.ToString()))
                .ToListAsync();
            return PartialView("_CreateDialog", new Application
            {
                VacancyId = vacancyId
            });
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,VacancyId")] Application application)
        {
            if (ModelState.IsValid)
            {
                application.ApplicationStatusId =
                    (await _context.ApplicationStatuses.FirstOrDefaultAsync(x => x.Name == "New")).ApplicationStatusId;
                application.CreateDate = DateTime.Now;
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Vacancies");
            }
            return BadRequest(ModelState);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "ApplicationStatusId", "Name", application.ApplicationStatusId);
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "CandidateId", "FirstName", application.CandidateId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Requirements", application.VacancyId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,CandidateId,VacancyId,ApplicationStatusId,CreateDate,UpdateDate")] Application application)
        {
            if (id != application.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "ApplicationStatusId", "Name", application.ApplicationStatusId);
            ViewData["CandidateId"] = new SelectList(_context.Candidates, "CandidateId", "FirstName", application.CandidateId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Requirements", application.VacancyId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.ApplicationStatus)
                .Include(a => a.Candidate)
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Applications/{applicationId}/Status/{applicationStatusId}")]
        public async Task<IActionResult> SetStatus(int applicationId, int applicationStatusId)
        {
                try
                {
                    var application = await _context.Applications.FindAsync(applicationId);
                    application.ApplicationStatusId = applicationStatusId;
                    if (applicationStatusId == (int) ApplicationStatusEnum.Hired)
                    {
                        var vacancy = await _context.Vacancies.Include(x => x.Applications).FirstOrDefaultAsync(x => x.VacancyId == application.VacancyId);
                        if (vacancy.Applications.Count(x =>
                            x.ApplicationStatusId == (int) ApplicationStatusEnum.Hired) >= vacancy.PositionsCount)
                        {
                            vacancy.EndDate = vacancy.UpdateDate = DateTime.Now;
                        }

                    }
                    application.UpdateDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(applicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationId == id);
        }
    }
}
