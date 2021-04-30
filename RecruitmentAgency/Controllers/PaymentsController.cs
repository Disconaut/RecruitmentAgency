using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly RecruiterAgencyContext _context;

        public PaymentsController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var recruiters = await _context.Employees
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Recruiter")
                .Select(x => new { Text = $"{x.FirstName} {x.LastName}", Value=x.EmployeeId.ToString(), x.DepartmentId})
                .ToListAsync();
            recruiters.Insert(0, new { Text = "All", Value = "-1", DepartmentId = 0 });
            ViewBag.Recruiters = recruiters;
            var years = await _context.Payments.Select(x => new SelectListItem(x.TransactionDate.Year.ToString(), x.TransactionDate.Year.ToString())).Distinct().ToListAsync();
            years.Insert(0, new SelectListItem("All", "-1"));
            ViewBag.Year = years;
            var departments = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            departments.Insert(0, new SelectListItem("All", "-1"));
            ViewBag.DepartmentId = departments;
            var customers = await _context.Customers
                .Select(x => new SelectListItem($"{x.FirstName} {x.LastName}", x.CustomerId.ToString()))
                .ToListAsync(); 
            customers.Insert(0, new SelectListItem("All", "-1"));
            ViewBag.CustomerId = customers;
            return View();
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Vacancy)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int? vacancyId)
        {
            if (!vacancyId.HasValue)
            {
                return BadRequest();
            }

            return PartialView("_CreateDialog", new Payment
            {
                VacancyId = vacancyId.Value,
                TransactionDate = DateTime.Now
            });
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,VacancyId,Sum,TransactionDate,CreateDate,UpdateDate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.CreateDate = DateTime.Now;
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Vacancies");
            }
            return BadRequest(ModelState);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Requirements", payment.VacancyId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,VacancyId,Sum,TransactionDate,CreateDate,UpdateDate")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "VacancyId", "Requirements", payment.VacancyId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Vacancy)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
