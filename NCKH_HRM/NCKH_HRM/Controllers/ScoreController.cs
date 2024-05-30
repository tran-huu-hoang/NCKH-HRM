﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;

namespace NCKH_HRM.Controllers
{
    public class ScoreController : BaseController
    {
        private readonly NckhDbContext _context;

        public ScoreController(NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            /*return _context.Terms != null ?
                        View(await _context.Terms.ToListAsync()) :
                        Problem("Entity set 'NckhDbContext.Terms'  is null.");*/

            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));

            var data = await (from userstaff in _context.UserStaffs
                              join staff in _context.Staff on userstaff.Staff equals staff.Id
                              join teachingassignment in _context.TeachingAssignments on staff.Id equals teachingassignment.Staff
                              join detailterm in _context.DetailTerms on teachingassignment.DetailTerm equals detailterm.Id
                              join term in _context.Terms on detailterm.Term equals term.Id
                              where staff.Id == user_staff.Id
                              select new Term
                              {
                                  Id = term.Id,
                                  Name = term.Name,
                              }).ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> EnterScore(long? id)
        {
            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join student in _context.Students on registstudent.Student equals student.Id
                              join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join attendance in _context.Attendances on detailterm.Id equals attendance.Id
                              join year in _context.Years on timeline.Year equals year.Id
                              join pointprocess in _context.PointProcesses on registstudent.Id equals pointprocess.RegistStudent
                              where term.Id == id && year.Name == DateTime.Now.Year
                              group new { student, timeline, attendance, pointprocess } by new
                              { student.Code, student.Name, timeline.DateLearn, attendanceId = attendance.Id,
                                  pointprocessId = pointprocess.Id, pointprocess.ComponentPoint, pointprocess.MidtermPoint,
                                  pointprocess.TestScore, pointprocess.Student, pointprocess.DetailTerm, pointprocess.RegistStudent,
                                  pointprocess.Attendance, pointprocess.NumberTest, pointprocess.IdStaff,
                                  pointprocess.CreateBy, pointprocess.UpdateBy, pointprocess.CreateDate,
                                  pointprocess.UpdateDate, pointprocess.IsDelete, pointprocess.IsActive} into g
                              select new EnterScore
                              {
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  PointId = g.Key.pointprocessId,
                                  ComponentPoint = g.Key.ComponentPoint,
                                  MidtermPoint = g.Key.MidtermPoint,
                                  TestScore = g.Key.TestScore,
                                  Student = g.Key.Student,
                                  DetailTerm = g.Key.DetailTerm,
                                  RegistStudent = g.Key.RegistStudent,
                                  Attendance = g.Key.Attendance,
                                  NumberTest = g.Key.NumberTest,
                                  IdStaff = g.Key.IdStaff,
                                  CreateBy = g.Key.CreateBy,
                                  UpdateBy = g.Key.UpdateBy,
                                  CreateDate = g.Key.CreateDate,
                                  UpdateDate = g.Key.UpdateDate,
                                  IsDelete = g.Key.IsDelete,
                                  IsActive = g.Key.IsActive,
                              }).ToListAsync();
            var termName = await _context.Terms.FindAsync(id);
            ViewBag.TermName = termName.Name;
            return View(data);  
        }

        [HttpPost]
        public async Task<IActionResult> EnterScore(IFormCollection form)
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));
            int itemCount = form["Attendance"].Count;
            for (int i = 0; i < itemCount; i++)
            {
                PointProcess pointProcess = new PointProcess();
                pointProcess.Id = long.Parse(form["PointId"][i]);
                pointProcess.Attendance = long.Parse(form["Attendance"][i]);
                pointProcess.DetailTerm = long.Parse(form["DetailTerm"][i]);
                pointProcess.Student = long.Parse(form["Student"][i]);
                pointProcess.RegistStudent = long.Parse(form["RegistStudent"][i]);
                pointProcess.ComponentPoint = Double.Parse(form["ComponentPoint"][i]);
                pointProcess.MidtermPoint = Double.Parse(form["MidtermPoint"][i]);
                pointProcess.TestScore = Double.Parse(form["TestScore"][i]);
                Double valueToRound = (pointProcess.ComponentPoint ?? 0) * 0.1 + (pointProcess.MidtermPoint ?? 0) * 0.3 + (pointProcess.TestScore ?? 0) * 0.6;
                pointProcess.OverallScore = Math.Round(valueToRound, 2);
                if(pointProcess.OverallScore >=4)
                {
                    pointProcess.Status = true;
                }else
                {
                    pointProcess.Status=false;
                }
                pointProcess.NumberTest = 1;
                pointProcess.IdStaff = user_staff.Staff;
                pointProcess.CreateBy = form["CreateBy"][i].ToString();
                pointProcess.UpdateBy = user_staff.Username;
                pointProcess.CreateDate = DateTime.Parse(form["CreateDate"][i]);
                pointProcess.UpdateDate = DateTime.Now;
                pointProcess.IsActive = bool.Parse(form["IsActive"][i]);
                pointProcess.IsDelete = bool.Parse(form["IsDelete"][i]);

                _context.Update(pointProcess);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}