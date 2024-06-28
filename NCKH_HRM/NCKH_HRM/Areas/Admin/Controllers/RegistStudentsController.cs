﻿using System;
using System.Collections;
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
    public class RegistStudentsController : BaseController
    {
        private readonly NckhDbContext _context;

        public RegistStudentsController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RegistStudents
        public async Task<IActionResult> Index(int page = 1)
        {
            //số bản ghi trên 1 trang
            int limit = 5;

            var registStudent = await _context.RegistStudents.Include(r => r.DetailTermNavigation).Include(r => r.StudentNavigation).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.RegistStudents == null)
            {
                return NotFound();
            }

            var registStudent = await _context.RegistStudents
                .Include(r => r.DetailTermNavigation)
                .Include(r => r.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registStudent == null)
            {
                return NotFound();
            }

            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "TermClass", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Create
        public async Task<IActionResult> Create()
        {
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "TermClass");
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }
        // POST: Admin/RegistStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student,DetailTerm,Relearn,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] RegistStudent registStudent)
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
                registStudent.CreateBy = admin.Username;
                registStudent.UpdateBy = admin.Username;
                registStudent.IsDelete = false;
                _context.Add(registStudent);
                await _context.SaveChangesAsync();

                var dataAttendance = _context.Attendances.Where(c => c.Id == registStudent.Id).FirstOrDefault();
                Attendance at = new Attendance();
                at.Student = registStudent.Student;
                at.DetailTerm = registStudent.DetailTerm;
                at.RegistStudent = registStudent.Id;
                at.Status = true;
                at.CreateBy = admin.Username;
                at.UpdateBy = admin.Username;
                at.IsDelete = false;
                _context.Add(at);
                await _context.SaveChangesAsync();
                dataAttendance = at;

                var dataPoint = _context.PointProcesses.Where(c => c.Id == registStudent.Id).FirstOrDefault();
                PointProcess pt = new PointProcess();
                pt.Student = registStudent.Student;
                pt.DetailTerm = registStudent.DetailTerm;
                pt.RegistStudent = registStudent.Id;
                pt.Attendance = dataAttendance.Id;
                pt.ComponentPoint = null;
                pt.MidtermPoint = null;
                pt.TestScore = null;
                pt.OverallScore = null;
                pt.NumberTest = null;
                pt.Status = null;
                pt.IdStaff = null;
                pt.CreateBy = admin.Username;
                pt.UpdateBy = admin.Username;
                pt.IsDelete = false;
                _context.Add(pt);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "TermClass", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.RegistStudents == null)
            {
                return NotFound();
            }

            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent == null)
            {
                return NotFound();
            }
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "TermClass", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // POST: Admin/RegistStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Student,DetailTerm,Relearn,Status,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] RegistStudent registStudent)
        {
            if (id != registStudent.Id)
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

                    var user = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    registStudent.UpdateBy = user.Username;
                    registStudent.UpdateDate = DateTime.Now;

                    _context.Update(registStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistStudentExists(registStudent.Id))
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
            ViewData["DetailTerm"] = new SelectList(_context.DetailTerms, "Id", "TermClass", registStudent.DetailTerm);
            ViewData["Student"] = new SelectList(_context.Students, "Id", "Name", registStudent.Student);
            return View(registStudent);
        }

        // GET: Admin/RegistStudents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            /*if (id == null || _context.RegistStudents == null)
            {
                return NotFound();
            }

            var registStudent = await _context.RegistStudents
                .Include(r => r.DetailTermNavigation)
                .Include(r => r.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registStudent == null)
            {
                return NotFound();
            }

            return View(registStudent);*/
            if (_context.RegistStudents == null)
            {
                return Problem("Entity set 'NckhDbContext.RegistStudents'  is null.");
            }
            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent != null)
            {
                _context.RegistStudents.Remove(registStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RegistStudents/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.RegistStudents == null)
            {
                return Problem("Entity set 'NckhDbContext.RegistStudents'  is null.");
            }
            var registStudent = await _context.RegistStudents.FindAsync(id);
            if (registStudent != null)
            {
                _context.RegistStudents.Remove(registStudent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool RegistStudentExists(long id)
        {
            return (_context.RegistStudents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}