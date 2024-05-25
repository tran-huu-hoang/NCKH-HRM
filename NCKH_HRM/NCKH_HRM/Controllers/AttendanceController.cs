using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace NCKH_HRM.Controllers
{
    public class AttendanceController : BaseController
    {
        private readonly NckhDbContext _context;

        public AttendanceController (NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));

            var data = await (from userstaff in _context.UserStaffs
                              join staff in _context.Staff on userstaff.Staff equals staff.Id
                              join teachingassignment in _context.TeachingAssignments on staff.Id equals teachingassignment.Staff
                              join detailterm in _context.DetailTerms on teachingassignment.DetailTerm equals detailterm.Id
                              join term in _context.Terms on detailterm.Term equals term.Id
                              join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join year in _context.Years on timeline.Year equals year.Id
                              join staffsubject in _context.StaffSubjects on staff.Id equals staffsubject.Staff
                              join subject in _context.Subjects on staffsubject.Staff equals subject.Id
                              where staff.Id == user_staff.Id && year.Name == DateTime.Now.Year
                              select new StaffIndex
                              {
                                  TermId = term.Id,
                                  StaffCode = staff.Code,
                                  StaffName = staff.Name,
                                  SubjectName = subject.Name,
                                  TermName = term.Name,
                                  StartDate = detailterm.StartDate,
                                  EndDate = detailterm.EndDate,
                                  Room = detailterm.Room,
                                  CollegeCredit = term.CollegeCredit,
                              }).ToListAsync();

            return View(data);
        }
        
        public async Task<IActionResult> StudentInTerm(long? id)
        {
            var data = await(from term in _context.Terms
                             join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                             join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                             join student in _context.Students on registstudent.Student equals student.Id
                             join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                             join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                             join attendance in _context.Attendances on detailterm.Id equals attendance.Id
                             where term.Id == id && timeline.DateLearn.Value.Date == DateTime.Now.Date
                             group new { student, timeline, registstudent, datelearn, detailterm, attendance } by new 
                             { student.Code, student.Name, timeline.DateLearn, student.Id, attendanceId = attendance.Id, detailtermId = detailterm.Id, datelearnId = datelearn.Id} into g
                             select new StudentInTerm
                             {
                                 StudentCode = g.Key.Code,
                                 StudentName = g.Key.Name,
                                 DateLearn = g.Key.DateLearn,
                                 StudentId = g.Key.Id,
                                 AttendanceId = g.Key.attendanceId,
                                 DetailTermId = g.Key.detailtermId,
                                 DateLearnId = g.Key.datelearnId,
                             }).ToListAsync();
            var termName = await _context.Terms.FindAsync(id);
            ViewBag.TermName = termName.Name;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentInTerm(IFormCollection form)
        {
            int itemCount = form["AttendanceId"].Count;
            for (int i =0; i< itemCount; i ++)
            {
                DetailAttendance attendancedetail = new DetailAttendance();
                attendancedetail.IdAttendance = long.Parse(form["AttendanceId"][i]);
                attendancedetail.DetailTerm = long.Parse(form["DetailTermId"][i]);
                attendancedetail.DateLearn = long.Parse(form["DateLearnId"][i]);
                attendancedetail.Status = int.Parse(form[(i+1).ToString()]);
                    
                _context.Add(attendancedetail);
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
