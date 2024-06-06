using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;

namespace NCKH_HRM.Controllers
{
    public class TimeTableController : BaseController
    {
        private readonly NckhDbContext _context;

        public TimeTableController(NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));

            var data = await(from userstaff in _context.UserStaffs
                             join staff in _context.Staff on userstaff.Staff equals staff.Id
                             join teachingassignment in _context.TeachingAssignments on staff.Id equals teachingassignment.Staff
                             join detailterm in _context.DetailTerms on teachingassignment.DetailTerm equals detailterm.Id
                             join term in _context.Terms on detailterm.Term equals term.Id
                             join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                             join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                             join year in _context.Years on timeline.Year equals year.Id
                             join detailattendances in _context.DetailAttendances on datelearn.Id equals detailattendances.DateLearn
                             where userstaff.Id == user_staff.Id && year.Name == DateTime.Now.Year
                             group new {timeline, detailterm, datelearn, detailattendances } by new
                             {
                                 timeline.DateLearn,
                                 detailterm.Room,
                                 datelearn.Id,
                             } into g
                             select new TimeTable
                             {
                                 DateLearn = g.Key.DateLearn,
                                 Room = g.Key.Room,
                                 PresentStudent = _context.DetailAttendances
                                                      .Count(da => da.DateLearn == g.Key.Id && (da.Status == 1 || da.Status == 3)),
                                 TotalStudent = _context.DetailAttendances
                                                      .Count(da => da.DateLearn == g.Key.Id),
                             }).ToListAsync();

            return View(data);
        }
    }
}
