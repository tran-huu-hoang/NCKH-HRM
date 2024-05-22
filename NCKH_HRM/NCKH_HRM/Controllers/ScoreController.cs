using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;

namespace NCKH_HRM.Controllers
{
    public class ScoreController : Controller
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

        public async Task<IActionResult> ScoreStudent(long? id)
        {
            
            return View();
        }
    }
}
