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
    public class CustomersController : Controller
    {
        private readonly RecruiterAgencyContext _context;

        public CustomersController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Customer> recruiterAgencyContext = _context.Customers
                .Include(c => c.Company);
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.FirstName.Contains(searchQuery)
            //        || x.LastName.Contains(searchQuery)
            //        || x.Company.Name.Contains(searchQuery)
            //        || x.PhoneNumber.Contains(searchQuery)
            //        || x.Email.Contains(searchQuery)
            //        || x.Company.Name.Contains(searchQuery));
            //}

            //var paginated = await Paginated<Customer>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;

            return View(await recruiterAgencyContext.ToListAsync());
        }

        [Route("{customerId}/Indebtedness")]
        public async Task<IActionResult> Indebtedness(int customerId, int pageIndex = 1, int pageSize = 10,
            string searchQuery = null)
        {
            var customer = await _context.Customers
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (customer == null)
            {
                return BadRequest();
            }

            IQueryable<Vacancy> recruiterAgencyContext = _context.Vacancies
                .Include(v => v.CompanyOffice)
                .ThenInclude(x => x.City)
                .Include(v => v.JobTitle)
                .ThenInclude(x => x.JobCategory)
                .Include(v => v.Recruiter)
                .Include(x => x.Tariff)
                .Include(x => x.Payments)
                .Where(x => x.Payments.Sum(x => x.Sum) < x.PositionsCount * x.Tariff.PriceForCandidate
                            && x.CustomerId == customerId
                            && x.RecruiterId == int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                            || User.IsInRole("Manager"));

            return View(new IndebtednessResponse
            {
                CustomerId = customer.CustomerId,
                CustomerFullName = $"{customer.FirstName} {customer.LastName}",
                CustomerCompany = $"{customer.Company.Name}",
                Vacancies = await recruiterAgencyContext.ToListAsync()
            });
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id, bool? showIndebtedness)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewData["ShowIndebtedness"] = showIndebtedness ?? true;
            return PartialView("_DetailsDialog" ,customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,CompanyId,Email,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreateDate = DateTime.Now;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", customer.CompanyId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", customer.CompanyId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,CompanyId,Email,PhoneNumber,Rating,CreateDate,UpdateDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customer.UpdateDate = DateTime.Now;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "Name", customer.CompanyId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
