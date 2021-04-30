using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;
using RecruitmentAgency.ViewModels;

namespace RecruitmentAgency.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly RecruiterAgencyContext _context;

        public VacanciesController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: Vacancies
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Vacancy> recruiterAgencyContext = _context.Vacancies
                .Include(v => v.CompanyOffice)
                .ThenInclude(x => x.Company)
                .Include(x => x.CompanyOffice.City)
                .Include(v => v.Customer)
                .Include(v => v.JobTitle)
                .ThenInclude(x => x.JobCategory)
                .Include(v => v.Recruiter)
                .Include(x => x.Applications)
                .Include(x => x.Payments)
                .Include(x => x.Tariff)
                .Where(x => x.RecruiterId == int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.CompanyOffice.Company.Name.Contains(searchQuery)
            //        || x.CompanyOffice.City.Name.Contains(searchQuery)
            //        || x.CompanyOffice.Address.Contains(searchQuery)
            //        || x.Customer.FirstName.Contains(searchQuery)
            //        || x.Customer.LastName.Contains(searchQuery)
            //        || x.JobTitle.Name.Contains(searchQuery));
            //}

            //var paginated = await Paginated<Vacancy>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;

            return View(await recruiterAgencyContext.ToListAsync());
        }

        // GET: Vacancies/1/Applications
        [Route("Vacancies/{vacancyId}/Applications")]
        public async Task<IActionResult> Applications(int vacancyId, int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            var vacancy = await _context.Vacancies
                .Include(x => x.CompanyOffice)
                .ThenInclude(x => x.Company)
                .Include(x => x.CompanyOffice.City)
                .Include(x => x.Customer)
                .Include(x => x.JobTitle)
                .ThenInclude(x => x.JobCategory)
                .FirstOrDefaultAsync(x =>
                    x.VacancyId == vacancyId &&
                    x.RecruiterId == int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (vacancy == null)
            {
                return Forbid();
            }

            IQueryable<Application> recruiterAgencyContext = _context.Applications
                .Include(x => x.Candidate)
                .ThenInclude(x => x.Department)
                .ThenInclude(x => x.City)
                .Include(x => x.ApplicationStatus)
                .Include(x => x.Vacancy)
                .Where(x => x.VacancyId == vacancyId);
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.Candidate.FirstName.Contains(searchQuery)
            //        || x.Candidate.LastName.Contains(searchQuery));
            //}

            //var paginated = await Paginated<Application>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;
            var response = new VacancyApplicationsResponse
            {
                VacancyId = vacancyId,
                VacancyCustomer = $"{vacancy.Customer.FirstName} {vacancy.Customer.LastName}",
                VacancyJobTitle = $"{vacancy.JobTitle.Name}",
                VacancyCompany = vacancy.CompanyOffice.Company.Name,
                VacancyCompanyOffice = $"{vacancy.CompanyOffice.City.Name}, {vacancy.CompanyOffice.Address}",
                Applications = await recruiterAgencyContext.ToListAsync(),
                VacancyClosed = vacancy.EndDate.HasValue
            };

            return View("Applications/Index", response);
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(int? id, bool? showViewApplications)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .Include(v => v.CompanyOffice)
                .ThenInclude(x => x.Company)
                .Include(x => x.CompanyOffice.City)
                .Include(v => v.Customer)
                .Include(v => v.JobTitle)
                .ThenInclude(x => x.JobCategory)
                .Include(v => v.Recruiter)
                .Include(x => x.Applications)
                .Include(x => x.Tariff)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(m => m.VacancyId == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            ViewData["ShowViewApplications"] = showViewApplications ?? true;
            return PartialView("_DetailsDialog", vacancy);
        }

        // GET: Vacancies/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CompanyOffice"] = await _context.CompanyOffices
                .Include(x => x.City)
                .Select(x => new { x.CompanyId, Value = x.CompanyOfficeId, Text = $"{x.City.Name}, {x.Address}"})
                .ToListAsync();
            ViewData["Customer"] = await _context.Customers
                .Select(x => new { x.CompanyId, Value = x.CustomerId, Text = $"{x.FirstName} {x.LastName}"})
                .ToListAsync();
            ViewData["JobTitle"] = await _context.JobTitles
                .Select(x => new {x.JobCategoryId, Value = x.JobTitleId, Text = x.Name})
                .ToListAsync();
            ViewData["TariffId"] = new SelectList(_context.Tariffs.Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now), "TariffId", "Name");
            ViewData["Tariff"] = await _context.Tariffs
                .Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new {Value = x.TariffId, Cost = x.PriceForCandidate})
                .ToListAsync();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["JobCategoryId"] = new SelectList(_context.JobCategories, "JobCategoryId", "Name");
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobTitleId,Requirements,PositionsCount,CustomerId,TariffId,CompanyOfficeId")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                vacancy.RecruiterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                vacancy.StartDate = vacancy.CreateDate = DateTime.Now;
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyOffice"] = await _context.CompanyOffices
                .Include(x => x.City)
                .Select(x => new { x.CompanyId, Value = x.CompanyOfficeId, Text = $"{x.City.Name}, {x.Address}" })
                .ToListAsync();
            ViewData["Customer"] = await _context.Customers
                .Select(x => new { x.CompanyId, Value = x.CustomerId, Text = $"{x.FirstName} {x.LastName}" })
                .ToListAsync();
            ViewData["JobTitle"] = await _context.JobTitles
                .Select(x => new { x.JobCategoryId, Value = x.JobTitleId, Text = x.Name })
                .ToListAsync();
            ViewData["TariffId"] = new SelectList(_context.Tariffs.Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now), "TariffId", "Name");
            ViewData["Tariff"] = await _context.Tariffs
                .Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new { Value = x.TariffId, Cost = x.PriceForCandidate })
                .ToListAsync();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["JobCategoryId"] = new SelectList(_context.JobCategories, "JobCategoryId", "Name");
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .Include(x => x.Applications)
                .ThenInclude(x => x.ApplicationStatus)
                .Include(x => x.CompanyOffice)
                .Include(x => x.JobTitle)
                .FirstOrDefaultAsync(x => x.VacancyId == id);
            if (vacancy == null)
            {
                return NotFound();
            }
            ViewData["CompanyOfficeId"] = await _context.CompanyOffices
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.CompanyOfficeId.ToString()))
                .ToListAsync();
            ViewData["CustomerId"] = await _context.Customers
                .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.CustomerId.ToString()))
                .ToListAsync();
            ViewData["JobTitleId"] = await _context.JobTitles
                .Select(x => new SelectListItem(x.Name, x.JobTitleId.ToString()))
                .ToListAsync();
            ViewData["TariffId"] = new SelectList(_context.Tariffs.Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now), "TariffId", "Name");
            ViewData["Tariff"] = await _context.Tariffs
                .Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new { Value = x.TariffId, Cost = x.PriceForCandidate })
                .ToListAsync();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["JobCategoryId"] = new SelectList(_context.JobCategories, "JobCategoryId", "Name");
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VacancyId,JobTitleId,Requirements,PositionsCount,StartDate,EndDate,CustomerId,TariffId,RecruiterId,CreateDate,UpdateDate,CompanyOfficeId")] Vacancy vacancy)
        {
            if (id != vacancy.VacancyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vacancy.UpdateDate = DateTime.Now;
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyExists(vacancy.VacancyId))
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
            ViewData["CompanyOfficeId"] = await _context.CompanyOffices
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.CompanyOfficeId.ToString()))
                .ToListAsync();
            ViewData["CustomerId"] = await _context.Customers
                .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.CustomerId.ToString()))
                .ToListAsync();
            ViewData["JobTitleId"] = await _context.JobTitles
                .Select(x => new SelectListItem(x.Name, x.JobTitleId.ToString()))
                .ToListAsync();
            ViewData["TariffId"] = new SelectList(_context.Tariffs.Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now), "TariffId", "Name");
            ViewData["Tariff"] = await _context.Tariffs
                .Where(x => x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new { Value = x.TariffId, Cost = x.PriceForCandidate })
                .ToListAsync();
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            ViewData["JobCategoryId"] = new SelectList(_context.JobCategories, "JobCategoryId", "Name");
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancies
                .Include(v => v.CompanyOffice)
                .Include(v => v.Customer)
                .Include(v => v.JobTitle)
                .Include(v => v.Recruiter)
                .Include(v => v.Tariff)
                .FirstOrDefaultAsync(m => m.VacancyId == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancies.Any(e => e.VacancyId == id);
        }
    }
}
