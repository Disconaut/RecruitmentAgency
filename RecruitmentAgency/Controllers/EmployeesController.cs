using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecruitmentAgency.Data;
using RecruitmentAgency.Models;
using RecruitmentAgency.ViewModels;

namespace RecruitmentAgency.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly RecruiterAgencyContext _context;

        public EmployeesController(RecruiterAgencyContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Employee> recruiterAgencyContext = _context.Employees
                .Include(e => e.Department)
                .ThenInclude(x => x.City)
                .Include(e => e.Role)
                .Include(x => x.PersonalAccount);
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.FirstName.Contains(searchQuery)
            //        || x.LastName.Contains(searchQuery)
            //        || x.Department.City.Name.Contains(searchQuery)
            //        || x.Department.Address.Contains(searchQuery)
            //        || x.PersonalAccount.Login.Contains(searchQuery)
            //        || x.Role.Name.Contains(searchQuery));
            //}

            //var paginated = await Paginated<Employee>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;

            return View(await recruiterAgencyContext.ToListAsync());
        }

        // GET: Employees/Recruiters
        [Route("Recruiters")]
        public async Task<IActionResult> Recruiters(int pageIndex = 1, int pageSize = 10, string searchQuery = null)
        {
            IQueryable<Employee> recruiterAgencyContext = _context.Employees
                .Include(e => e.Department)
                .ThenInclude(x => x.City)
                .Include(e => e.Role)
                .Include(x => x.Vacancies)
                .ThenInclude(x => x.Applications)
                .ThenInclude(x => x.ApplicationStatus)
                .Include(x => x.Vacancies)
                .ThenInclude(x => x.Applications)
                .ThenInclude(x => x.Interviews)
                .Where(x => x.Role.Name == "Recruiter");
            //if (!string.IsNullOrWhiteSpace(searchQuery))
            //{
            //    recruiterAgencyContext = recruiterAgencyContext.Where(x =>
            //        x.FirstName.Contains(searchQuery)
            //        || x.LastName.Contains(searchQuery)
            //        || x.Department.City.Name.Contains(searchQuery)
            //        || x.Department.Address.Contains(searchQuery));
            ////}

            //var paginated = await Paginated<Employee>.PaginateAsync(recruiterAgencyContext, pageIndex, pageSize);
            //paginated.SearchQuery = searchQuery;

            return View(await recruiterAgencyContext.ToListAsync());
        }

        // GET: Employees/Recruiters/Details/5
        [Route("Recruiters/Details/{id?}")]
        public async Task<IActionResult> RecruiterDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .ThenInclude(x => x.City)
                .Include(x => x.PersonalAccount)
                .Include(e => e.Role)
                .Where(x => x.Role.Name == "Recruiter")
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return PartialView("_RecruiterDetailsDialog", employee);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .ThenInclude(x => x.City)
                .Include(x => x.PersonalAccount)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsDialog", employee);
        }
        
        // GET: Employees/Recruiters/Create
        [Route("Recruiters/Create")]
        public async Task<IActionResult> CreateRecruiter()
        {
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View();
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name");
            return View();
        }
        
        // POST: Employees/Recruiters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Recruiters/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecruiter([Bind("EmployeeId,FirstName,LastName,DepartmentId,PersonalAccount")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.RoleId = await _context.Roles.Where(x => x.Name == "Recruiter").Select(x => x.RoleId).FirstOrDefaultAsync();
                employee.PersonalAccount.CreateDate = employee.CreateDate = DateTime.Now;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Recruiters));
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View(employee);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,DepartmentId,RoleId,Rating,CreateDate,PersonalAccount,UpdateDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreateDate = DateTime.Now;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name");
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(x => x.PersonalAccount)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name");
            return View(employee);
        }

        // GET: Employees/Recruiters/Edit/5
        [Route("Recruiters/Edit/{Id?}")]
        public async Task<IActionResult> EditRecruiter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(x => x.PersonalAccount)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (employee == null || employee.Role.Name != "Recruiter")
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,DepartmentId,PersonalAccount,RoleId,Rating,CreateDate,UpdateDate")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.UpdateDate = employee.PersonalAccount.UpdateDate = DateTime.Now;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Recruiters));
            }
            ViewData["DepartmentId"] = await _context.Departments
                .Include(x => x.City)
                .Select(x => new SelectListItem($"{x.City.Name}, {x.Address}", x.DepartmentId.ToString()))
                .ToListAsync();
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "Name");
            return View(employee);
        }

        // POST: Employees/Recruiters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Recruiters/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecruiter(int id, [Bind("EmployeeId,FirstName,LastName,DepartmentId,PersonalAccount,RoleId,Rating,CreateDate,UpdateDate")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.UpdateDate = DateTime.Now;
                    employee.PersonalAccount.UpdateDate = DateTime.Now;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet("~/Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("~/Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(PersonalAccount model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.PersonalAccounts
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(model);
        }

        [HttpPost("~/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        private async Task Authenticate(PersonalAccount account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Employee.EmployeeId.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Employee.Role.Name),
                new Claim(ClaimTypes.Name, account.Employee.FirstName),
                new Claim(ClaimTypes.Surname, account.Employee.LastName),
                new Claim(ClaimTypes.GivenName, account.Login)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
