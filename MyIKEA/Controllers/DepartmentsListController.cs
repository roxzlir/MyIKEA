using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyIKEA.Data;
using MyIKEA.Models;

namespace MyIKEA.Controllers
{
    public class DepartmentsListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DepartmentsList
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DepartmentsList.Include(d => d.Department).Include(d => d.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DepartmentsList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentList = await _context.DepartmentsList
                .Include(d => d.Department)
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(m => m.DepartmentListId == id);
            if (departmentList == null)
            {
                return NotFound();
            }

            return View(departmentList);
        }

        // GET: DepartmentsList/Create
        public IActionResult Create()
        {
            ViewData["FkDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName");
            return View();
        }

        // POST: DepartmentsList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentListId,FkEmployeeId,FkDepartmentId")] DepartmentList departmentList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FkDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", departmentList.FkDepartmentId);
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", departmentList.FkEmployeeId);
            return View(departmentList);
        }

        // GET: DepartmentsList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentList = await _context.DepartmentsList.FindAsync(id);
            if (departmentList == null)
            {
                return NotFound();
            }
            ViewData["FkDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", departmentList.FkDepartmentId);
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", departmentList.FkEmployeeId);
            return View(departmentList);
        }

        // POST: DepartmentsList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentListId,FkEmployeeId,FkDepartmentId")] DepartmentList departmentList)
        {
            if (id != departmentList.DepartmentListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentListExists(departmentList.DepartmentListId))
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
            ViewData["FkDepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", departmentList.FkDepartmentId);
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeName", departmentList.FkEmployeeId);
            return View(departmentList);
        }

        // GET: DepartmentsList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentList = await _context.DepartmentsList
                .Include(d => d.Department)
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(m => m.DepartmentListId == id);
            if (departmentList == null)
            {
                return NotFound();
            }

            return View(departmentList);
        }

        // POST: DepartmentsList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentList = await _context.DepartmentsList.FindAsync(id);
            if (departmentList != null)
            {
                _context.DepartmentsList.Remove(departmentList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentListExists(int id)
        {
            return _context.DepartmentsList.Any(e => e.DepartmentListId == id);
        }
    }
}
