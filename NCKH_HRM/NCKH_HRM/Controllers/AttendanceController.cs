﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NCKH_HRM.Controllers
{
    public class AttendanceController : BaseController
    {
        private readonly NckhDbContext _context;

        public AttendanceController(NckhDbContext context)
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
                              join subject in _context.Subjects on staffsubject.Subject equals subject.Id
                              where userstaff.Id == user_staff.Id && year.Name == DateTime.Now.Year
                              group new { term, staff, subject, detailterm } by new
                              {
                                  detailterm.Id,
                                  term.Name,
                                  term.Code,
                                  term.CollegeCredit,
                                  staffCode = staff.Code,
                                  staffName = staff.Name,
                                  subjectName = subject.Name,
                                  detailterm.StartDate,
                                  detailterm.EndDate,
                                  detailterm.Room,
                              } into g
                              select new StaffIndex
                              {
                                  DetailTermId = g.Key.Id,
                                  StaffCode = g.Key.Code,
                                  StaffName = g.Key.Name,
                                  SubjectName = g.Key.subjectName,
                                  TermName = g.Key.Name,
                                  TermCode = g.Key.Code,
                                  StartDate = g.Key.StartDate,
                                  EndDate = g.Key.EndDate,
                                  Room = g.Key.Room,
                                  CollegeCredit = g.Key.CollegeCredit,
                              }).ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> AttendanceSheet(long? id)
        {
            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join student in _context.Students on registstudent.Student equals student.Id
                              where detailterm.Id == 1
                              group new { student, detailattendance, detailterm } by new
                              {
                                  student.Code,
                                  student.Name,
                              } into g
                              select new AttendanceSheet
                              {
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  Statuses = g.Select(x => x.detailattendance.Status ?? -1).ToList(),
                                  numberOfClassesAttended = g.Count(x => x.detailattendance.Status == 1 || x.detailattendance.Status == 3)
                              }).ToListAsync();

            var dateLearn = await (
                              from detailterm in _context.DetailTerms
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              where detailterm.Id == id
                              group new { timeline, registstudent, datelearn, detailterm, attendance, detailattendance } by new
                              {
                                  timeline.DateLearn,
                              } into g
                              select new Timeline
                              {
                                  DateLearn = g.Key.DateLearn,

                              }).ToListAsync();


            var termName = (from detailterm in _context.DetailTerms
                            join term in _context.Terms on detailterm.Term equals term.Id
                            where detailterm.Id == id
                            select new NameTermWithIdDT
                            {
                                Id = detailterm.Id,
                                Name = term.Name
                            }).FirstOrDefault();
            ViewBag.TermName = termName.Name;
            ViewBag.dateLearn = dateLearn;
            ViewBag.detailTerm = id;
            ViewBag.countDateLearn = dateLearn.Count;

            return View(data);
        }

        public async Task<IActionResult> StudentInTerm(long? id)
        {
            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join student in _context.Students on registstudent.Student equals student.Id
                              where detailterm.Id == id && timeline.DateLearn.Value.Date == DateTime.Now.Date
                              group new { student, timeline, registstudent, datelearn, detailterm, attendance, detailattendance } by new
                              {
                                  student.Code,
                                  student.Name,
                                  timeline.DateLearn,
                                  student.Id,
                                  attendanceId = attendance.Id,
                                  detailtermId = detailterm.Id,
                                  datelearnId = datelearn.Id,
                                  detailattendance.Status,
                                  detailattendanceId = detailattendance.Id
                              } into g
                              select new StudentInTerm
                              {
                                  Id = g.Key.detailattendanceId,
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  DateLearn = g.Key.DateLearn,
                                  StudentId = g.Key.Id,
                                  AttendanceId = g.Key.attendanceId,
                                  DetailTermId = g.Key.detailtermId,
                                  DateLearnId = g.Key.datelearnId,
                                  Status = g.Key.Status,
                              }).ToListAsync();
            var termName = (from detailterm in _context.DetailTerms
                            join term in _context.Terms on detailterm.Term equals term.Id
                            where detailterm.Id == id
                            select new NameTermWithIdDT
                            {
                                Id = detailterm.Id,
                                Name = term.Name
                            }).FirstOrDefault();
            ViewBag.TermName = termName.Name;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentInTerm(IFormCollection form)
        {
            int itemCount = form["AttendanceId"].Count;
            for (int i = 0; i < itemCount; i++)
            {
                DetailAttendance attendancedetail = new DetailAttendance();
                attendancedetail.Id = long.Parse(form["Id"][i]);
                attendancedetail.IdAttendance = long.Parse(form["AttendanceId"][i]);
                attendancedetail.DetailTerm = long.Parse(form["DetailTermId"][i]);
                attendancedetail.DateLearn = long.Parse(form["DateLearnId"][i]);
                attendancedetail.Status = int.Parse(form[(i + 1).ToString()]);

                _context.Update(attendancedetail);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, IFormCollection form)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 0; row < rowCount; row++)
                    {
                        DetailAttendance detailAttendance = new DetailAttendance();
                        detailAttendance.Id = long.Parse(form["Id"][row]);
                        detailAttendance.IdAttendance = long.Parse(form["AttendanceId"][row]);
                        detailAttendance.DetailTerm = long.Parse(form["DetailTermId"][row]);
                        detailAttendance.DateLearn = long.Parse(form["DateLearnId"][row]);
                        detailAttendance.Status = int.Parse(worksheet.Cells[row + 1, 3].Value.ToString().Trim());
                        _context.Update(detailAttendance);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Attendance");
        }
    }
}