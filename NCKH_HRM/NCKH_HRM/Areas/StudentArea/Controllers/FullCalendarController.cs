using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Areas.StudentArea.Controllers;
using NCKH_HRM.Models;
using NCKH_HRM.Areas.StudentArea;
using Newtonsoft.Json;

namespace NCKH_HRM.Areas.Controllers
{
    public class FullCalendarController : BaseController
    {
        private readonly NckhDbContext _context;

        public FullCalendarController(NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StudentLogin"));

            var data = await (from userstudent in _context.UserStudents
                              join student in _context.Students on userstudent.Student equals student.Id
                              join registstudent in _context.RegistStudents on student.Id equals registstudent.Student
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join detailterm in _context.DetailTerms on registstudent.DetailTerm equals detailterm.Id
                              join term in _context.Terms on detailterm.Term equals term.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              /*join year in _context.Years on timeline.Year equals year.Id*/
                              where userstudent.Id == user_staff.Id
                              group new { term, timeline, detailterm, detailattendance } by new
                              {
                                  term.Name,
                                  timeline.DateLearn,
                                  detailterm.Room,
                                  detailattendance.BeginClass,
                                  detailattendance.EndClass
                              } into g
                              select new ViewModels.FullCalendarVM
                              {
                                  Name = g.Key.Name,
                                  DateLearn = g.Key.DateLearn,
                                  DateOnly = DateOnly.FromDateTime(g.Key.DateLearn.Value),
                                  TimeStart = TimeOnly.FromDateTime(g.Key.DateLearn.Value),
                                  TimeEnd = TimeOnly.FromDateTime(g.Key.DateLearn.Value).AddHours(3).AddMinutes(30),
                                  Room = g.Key.Room,
                                  BeginClass = g.Key.BeginClass,
                                  EndClass = g.Key.EndClass
                              }).ToListAsync();

            return View(data);
        }
    }
}