using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;
using X.PagedList;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AttendancesController : BaseController
    {
        private readonly NckhDbContext _context;

        public AttendancesController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Attendances
        public async Task<IActionResult> Index(int page = 1)
        {
            //số bản ghi trên 1 trang
            int limit = 5;
            var nckhDbContext = _context.Attendances.Include(a => a.DetailTermNavigation).Include(a => a.RegistStudentNavigation).Include(a => a.StudentNavigation);
            var account = await nckhDbContext.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            ViewBag.Term = await _context.Terms.ToListAsync();
            return View(account);
        }

        // GET: Admin/Attendances/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.DetailTermNavigation)
                .Include(a => a.RegistStudentNavigation)
                .Include(a => a.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }
            var data = await (from detailterm in _context.DetailTerms
                              join term in _context.Terms on detailterm.Term equals term.Id
                              select new NameTermWithIdDT
                              {
                                  Id = detailterm.Id,
                                  Name = term.Name
                              }).ToListAsync();

            ViewData["DetailTerm"] = new SelectList(data, "Id", "Name");
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", attendance.RegistStudent);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", attendance.Student);
            return View(attendance);
        }

        // GET: Admin/Attendances/Create
        public IActionResult Create()
        {
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id");
            return View();
        }

        // POST: Admin/Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,DetailTerm,RegistStudent,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var userStaffSession = HttpContext.Session.GetString("AdminLogin");
                if (string.IsNullOrEmpty(userStaffSession))
                {
                    // Handle the case where the session is missing
                    return RedirectToAction(actionName: "Index", controllerName: "Login");
                }
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                attendance.CreateBy = admin.Username;
                attendance.UpdateBy = admin.Username;
                attendance.IsDelete = false;
                attendance.Status = false;

                RegistStudent rs = await _context.RegistStudents.FindAsync(attendance.RegistStudent);
                attendance.Student = rs.Student;
                attendance.DetailTerm = rs.DetailTerm;

                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", attendance.RegistStudent);
            return View(attendance);
        }

        // GET: Admin/Attendances/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", attendance.RegistStudent);
            return View(attendance);
        }

        // POST: Admin/Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Student,DetailTerm,RegistStudent,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userStaffSession = HttpContext.Session.GetString("AdminLogin");
                    if (string.IsNullOrEmpty(userStaffSession))
                    {
                        // Handle the case where the session is missing
                        return RedirectToAction(actionName: "Index", controllerName: "Login");
                    }

                    attendance.UpdateDate = DateTime.Now;
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    attendance.UpdateBy = admin.Username;
                    RegistStudent rs = await _context.RegistStudents.FindAsync(attendance.RegistStudent);
                    attendance.Student = rs.Student;
                    attendance.DetailTerm = rs.DetailTerm;
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", attendance.RegistStudent);
            return View(attendance);
        }

        // GET: Admin/Attendances/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            /*if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.DetailTermNavigation)
                .Include(a => a.RegistStudentNavigation)
                .Include(a => a.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);*/

            if (_context.Attendances == null)
            {
                return Problem("Entity set 'NckhDbContext.Attendances'  is null.");
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Attendances/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Attendances == null)
            {
                return Problem("Entity set 'NckhDbContext.Attendances'  is null.");
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool AttendanceExists(long id)
        {
            return (_context.Attendances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}