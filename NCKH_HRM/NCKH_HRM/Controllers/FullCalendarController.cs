using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;

namespace NCKH_HRM.Controllers
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
                              where userstaff.Id == user_staff.Id
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