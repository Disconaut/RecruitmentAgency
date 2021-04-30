using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;
using RecruitmentAgency.ViewModels;

namespace RecruitmentAgency.Controllers
{
    [Route("api/Payments")]
    [ApiController]
    public class PaymentsApiController : ControllerBase
    {
        private readonly RecruiterAgencyContext _context;

        public PaymentsApiController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: api/PaymentsApi
        [HttpGet]
        public async Task<ActionResult<decimal>> GetSum([FromQuery]PaymentFilter filter)
        {
            return await _context.Payments
                .Where(x => (!filter.Year.HasValue || filter.Year == -1 || x.TransactionDate.Year == filter.Year)
                    && (!filter.Month.HasValue || filter.Month == -1 || x.TransactionDate.Month == filter.Month)
                    && (!filter.Day.HasValue || filter.Day == -1 || x.TransactionDate.Day == filter.Day)
                    && (!filter.DepartmentId.HasValue || filter.DepartmentId == -1 || x.Vacancy.Recruiter.DepartmentId == filter.DepartmentId)
                    && (!filter.RecruiterId.HasValue || filter.RecruiterId == -1 || x.Vacancy.RecruiterId == filter.RecruiterId)
                    && (!filter.CustomerId.HasValue || filter.CustomerId == -1 || x.Vacancy.CustomerId == filter.CustomerId))
                .SumAsync(x => x.Sum);
        }

        // GET: api/PaymentsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/PaymentsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.PaymentId }, payment);
        }

        // DELETE: api/PaymentsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
