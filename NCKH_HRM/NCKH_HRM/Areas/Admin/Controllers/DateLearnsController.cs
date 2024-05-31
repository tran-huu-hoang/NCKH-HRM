using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DateLearnsController : BaseController
    {
        private readonly NckhDbContext _context;

        public DateLearnsController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DateLearns
        public async Task<IActionResult> Index(int page = 1)
        {
            //số bản ghi trên 1 trang
            int limit = 5;

            var dateLearn = await _context.DateLearns.Include(d => d.DetailTermNavigation).Include(d => d.RegistStudentNavigation).Include(d => d.StudentNavigation).Include(d => d.TimelineNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit); ;
            ViewBag.Term = await _context.Terms.ToListAsync();
            return View(dateLearn);
        }
        // GET: Admin/DateLearns/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DateLearns == null)
            {
                return NotFound();
            }

            var dateLearn = await _context.DateLearns
                .Include(d => d.DetailTermNavigation)
                .Include(d => d.RegistStudentNavigation)
                .Include(d => d.StudentNavigation)
                .Include(d => d.TimelineNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dateLearn == null)
            {
                return NotFound();
            }
            ViewData["DetailTerm"] = new SelectList(_context.Terms, "Id", "Name", dateLearn.DetailTerm);
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", dateLearn.RegistStudent);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", dateLearn.Student);
            ViewData["Timeline"] = new SelectList(_context.Timelines, "Id", "DateLearn", dateLearn.Timeline);
            return View(dateLearn);
        }

        // GET: Admin/DateLearns/Create
        public IActionResult Create()
        {
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id");
            ViewData["Timeline"] = new SelectList(_context.Timelines, "Id", "DateLearn");
            return View();
        }

        // POST: Admin/DateLearns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,DetailTerm,RegistStudent,Timeline,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] DateLearn dateLearn)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                dateLearn.CreateBy = admin.Username;
                dateLearn.UpdateBy = admin.Username;
                dateLearn.IsDelete = false;
                RegistStudent rs = await _context.RegistStudents.FindAsync(dateLearn.RegistStudent);
                dateLearn.Student = rs.Student;
                dateLearn.DetailTerm = rs.DetailTerm;
                _context.Add(dateLearn);
                await _context.SaveChangesAsync();

                var dataAttendance = _context.Attendances.Where(c => c.RegistStudent == dateLearn.RegistStudent).FirstOrDefault();
                DetailAttendance da = new DetailAttendance();
                da.IdAttendance = dataAttendance.Id;
                da.DateLearn = dateLearn.Id;
                da.DetailTerm = dateLearn.DetailTerm;
                da.Status = null;
                _context.Add(da);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", dateLearn.RegistStudent);
            ViewData["Timeline"] = new SelectList(_context.Timelines, "Id", "DateLearn", dateLearn.Timeline);
            return View(dateLearn);
        }

        // GET: Admin/DateLearns/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DateLearns == null)
            {
                return NotFound();
            }

            var dateLearn = await _context.DateLearns.FindAsync(id);
            if (dateLearn == null)
            {
                return NotFound();
            }
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", dateLearn.RegistStudent);
            ViewData["Timeline"] = new SelectList(_context.Timelines, "Id", "DateLearn", dateLearn.Timeline);
            return View(dateLearn);
        }

        // POST: Admin/DateLearns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Student,DetailTerm,RegistStudent,Timeline,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] DateLearn dateLearn)
        {
            if (id != dateLearn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    dateLearn.UpdateBy = user.Username;
                    dateLearn.UpdateDate = DateTime.Now;
                    RegistStudent rs = await _context.RegistStudents.FindAsync(dateLearn.RegistStudent);
                    dateLearn.Student = rs.Student;
                    dateLearn.DetailTerm = rs.DetailTerm;
                    _context.Update(dateLearn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DateLearnExists(dateLearn.Id))
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
            ViewData["RegistStudent"] = new SelectList(_context.RegistStudents, "Id", "Id", dateLearn.RegistStudent);
            ViewData["Timeline"] = new SelectList(_context.Timelines, "Id", "DateLearn", dateLearn.Timeline);
            return View(dateLearn);
        }

        // GET: Admin/DateLearns/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            /* if (id == null || _context.DateLearns == null)
             {
                 return NotFound();
             }

             var dateLearn = await _context.DateLearns
                 .Include(d => d.DetailTermNavigation)
                 .Include(d => d.RegistStudentNavigation)
                 .Include(d => d.StudentNavigation)
                 .Include(d => d.TimelineNavigation)
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (dateLearn == null)
             {
                 return NotFound();
             }

             return View(dateLearn);*/
            if (_context.DateLearns == null)
            {
                return Problem("Entity set 'NckhDbContext.DateLearns'  is null.");
            }
            var dateLearn = await _context.DateLearns.FindAsync(id);
            if (dateLearn != null)
            {
                _context.DateLearns.Remove(dateLearn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/DateLearns/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.DateLearns == null)
            {
                return Problem("Entity set 'NckhDbContext.DateLearns'  is null.");
            }
            var dateLearn = await _context.DateLearns.FindAsync(id);
            if (dateLearn != null)
            {
                _context.DateLearns.Remove(dateLearn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool DateLearnExists(long id)
        {
            return (_context.DateLearns?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
