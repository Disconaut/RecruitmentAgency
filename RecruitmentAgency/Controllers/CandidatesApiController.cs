using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;

namespace RecruitmentAgency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesApiController : ControllerBase
    {
        private readonly RecruiterAgencyContext _context;

        public CandidatesApiController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: api/CandidatesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            return await _context.Candidates.ToListAsync();
        }

        // GET: api/CandidatesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return candidate;
        }

        // PUT: api/CandidatesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidate(int id, Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return BadRequest();
            }

            _context.Entry(candidate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateExists(id))
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

        // POST: api/CandidatesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Candidate>> PostCandidate(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCandidate", new { id = candidate.CandidateId }, candidate);
        }

        // DELETE: api/CandidatesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
