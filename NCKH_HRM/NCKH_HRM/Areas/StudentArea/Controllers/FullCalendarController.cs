using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Areas.StudentArea.Controllers;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
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
                              join detailterm in _context.DetailTerms on registstudent.DetailTerm equals detailterm.Id
                              join term in _context.Terms on detailterm.Term equals term.Id
                              join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join year in _context.Years on timeline.Year equals year.Id
                              where userstudent.Id == user_staff.Id
                              group new { term, timeline, detailterm } by new
                              {
                                  term.Name,
                                  timeline.DateLearn,
                                  detailterm.Room,
                                  datelearn.Id
                              } into g
                              select new FullCalendarVM
                              {
                                  Name = g.Key.Name,
                                  DateLearn = g.Key.DateLearn,
                                  DateOnly = DateOnly.FromDateTime(g.Key.DateLearn.Value),
                                  TimeStart = TimeOnly.FromDateTime(g.Key.DateLearn.Value),
                                  TimeEnd = TimeOnly.FromDateTime(g.Key.DateLearn.Value).AddHours(3).AddMinutes(30),
                                  Room = g.Key.Room,
                              }).ToListAsync();

            return View(data);
        }
    }
}