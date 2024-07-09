using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Areas.StudentArea.ViewModels;
using NCKH_HRM.Models;
using Newtonsoft.Json;

namespace NCKH_HRM.Areas.StudentArea.Controllers
{
    [Area("StudentArea")]
    public class ScoreController : BaseController
    {
        private readonly NckhDbContext _context;

        public ScoreController(NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user_student = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StudentLogin"));

            var data = await (from userstudent in _context.UserStudents
                              join student in _context.Students on userstudent.Student equals student.Id
                              join pointprocesses in _context.PointProcesses on student.Id equals pointprocesses.Student
                              join detailterm in _context.DetailTerms on pointprocesses.DetailTerm equals detailterm.Id
                              join term in _context.Terms on detailterm.Term equals term.Id
                              join semesters in _context.Semesters on detailterm.Semester equals semesters.Id
                              where userstudent.Id == user_student.Id
                              group new { term, detailterm, semesters, pointprocesses } by new
                              {
                                  detailterm.Id,
                                  termName = term.Name,
                                  term.Code,
                                  term.CollegeCredit,
                                  semesters.Name,
                                  pointprocesses.OverallScore,
                              } into g
                              select new StudentScore
                              {
                                  DetailTermId = g.Key.Id,
                                  Semester = g.Key.Name,
                                  TermCode = g.Key.Code,
                                  TermName = g.Key.termName,
                                  CollegeCredit = g.Key.CollegeCredit == null ? null : g.Key.CollegeCredit,
                                  PointRange10 = g.Key.OverallScore,
                                  PointRange4 = g.Key.OverallScore >= 8.5 ? 4.0 :
                                                g.Key.OverallScore >= 7.0 ? 3.0 :
                                                g.Key.OverallScore >= 5.5 ? 2.0 :
                                                g.Key.OverallScore >= 4.0 ? 1.0 :
                                                g.Key.OverallScore == null ? null : 0.0
                              }).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> ScoreDetail(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user_student = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StudentLogin"));

            var data = (from userstudents in _context.UserStudents
                        join students in _context.Students on userstudents.Student equals students.Id
                        join registstudents in _context.RegistStudents on students.Id equals registstudents.Student
                        join detailterm in _context.DetailTerms on registstudents.DetailTerm equals detailterm.Id
                        join semesters in _context.Semesters on detailterm.Semester equals semesters.Id
                        join attendances in _context.Attendances on detailterm.Id equals attendances.DetailTerm
                        join detailattendances in _context.DetailAttendances on attendances.Id equals detailattendances.IdAttendance
                        join pointprocesses in _context.PointProcesses on registstudents.Id equals pointprocesses.RegistStudent
                        where detailterm.Id == id && userstudents.Id == user_student.Id
                        group new { detailterm, semesters, pointprocesses, detailattendances, registstudents } by new
                        {
                            semesters.Name,
                            pointprocesses.MidtermPoint,
                            pointprocesses.ComponentPoint,
                            pointprocesses.TestScore,
                            pointprocesses.NumberTest,
                            pointprocesses.OverallScore,
                            registstudents.Relearn,
                        } into g
                        select new StudentScoreDetail
                        {
                            Semester = g.Key.Name,
                            AttendancePoint = (double)(g.Count(x => x.detailattendances.BeginClass == 1) //đếm số buổi đầu giờ đi học
                            + g.Count(x => x.detailattendances.EndClass == 1) //đếm số buổi cuối giờ đi học
                            + (double)(g.Count(x => x.detailattendances.BeginClass == 4) + g.Count(x => x.detailattendances.EndClass == 4)) / 2) //đếm số buổi muộn
                            / (g.Count(x => x.detailattendances.BeginClass.HasValue) * 2),//đếm số buổi học (đầu giờ + cuối giờ)
                            MidtermPoint = g.Key.MidtermPoint,
                            ComponentPoint = g.Key.ComponentPoint,
                            TestScore = g.Key.TestScore,
                            NumberTest = g.Key.NumberTest,
                            OverallScore = g.Key.OverallScore,
                            Relearn = g.Key.Relearn,
                        }).FirstOrDefault();

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }
    }
}
