using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;
using RecruitmentAgency.ViewModels;

namespace RecruitmentAgency.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly RecruiterAgencyContext _context;
        private readonly IWebHostEnvironment _webHost;

        public CandidatesController(RecruiterAgencyContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Candidates
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Candidate> recruiterAgencyContext = _context.Candidates
                .Include(c => c.Department)
                .ThenInclude(x => x.City);
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                recruiterAgencyContext = recruiterAgencyContext.Where(x =>
                    x.FirstName.Contains(searchQuery)
                    || x.LastName.Contains(searchQuery)
                    || x.Department.City.Name.Contains(searchQuery)
                    || x.Department.Address.Contains(searchQuery)
                    || x.PhoneNumber.Contains(searchQuery)
                    || x.Email.Contains(searchQuery));
            }

            var paginated = await Paginated<Candidate>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            paginated.SearchQuery = searchQuery;

            return View(await recruiterAgencyContext.ToListAsync());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Department)
                .ThenInclude(x => x.City)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsDialog", candidate);
        }

        // GET: Candidates/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                if (candidate.Resume != null)
                {
                    var filePath = "Resumes\\" + candidate.Resume.FileName;
                    await using (var stream =
                        new FileStream(Path.Combine(_webHost.WebRootPath, filePath), FileMode.Create))
                    {
                        await candidate.Resume.CopyToAsync(stream);
                    }

                    candidate.ResumeUrl = filePath;
                }

                candidate.CreateDate = DateTime.Now;
                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View(candidate);
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            try
            {
                await using var stream =
                    new FileStream(Path.Combine(_webHost.ApplicationName, candidate.ResumeUrl), FileMode.Open);
                candidate.Resume = new FormFile(stream, stream.Position, stream.Length,
                    Path.GetFileNameWithoutExtension(stream.Name), Path.GetFileName(stream.Name));
            }
            catch (Exception)
            {
                // ignore
            }

            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,FirstName,LastName,Rating,DepartmentId,Email,PhoneNumber,Resume,CreateDate")] Candidate candidate)
        {
            if (id != candidate.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (candidate.Resume != null)
                    {
                        var filePath = "Resumes\\" + candidate.Resume.FileName;
                        await using (var stream =
                            new FileStream(Path.Combine(_webHost.WebRootPath, filePath), FileMode.OpenOrCreate))
                        {
                            await candidate.Resume.CopyToAsync(stream);
                        }

                        candidate.ResumeUrl = filePath;
                    }

                    candidate.UpdateDate = DateTime.Now;
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.CandidateId))
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
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
