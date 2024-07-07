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
                                  termName = term.Name,
                                  term.Code,
                                  term.CollegeCredit,
                                  semesters.Name,
                                  pointprocesses.OverallScore,
                              } into g
                              select new StudentScore
                              {
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
    }
}
