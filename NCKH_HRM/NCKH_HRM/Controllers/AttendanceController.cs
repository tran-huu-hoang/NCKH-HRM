using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using System.Diagnostics;

namespace NCKH_HRM.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly NckhDbContext _context;
        private List<StudentInTerm> list = new List<StudentInTerm>();

        public AttendanceController (NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Terms != null ?
                        View(await _context.Terms.ToListAsync()) :
                        Problem("Entity set 'NckhDbContext.Terms'  is null.");
        }

        public async Task<IActionResult> StudentInTerm(long? id)
        {
            var data = await(from t1 in _context.Terms
                             join t2 in _context.DetailTerms on t1.Id equals t2.Term
                             join t3 in _context.RegistStudents on t2.Id equals t3.DetailTerm
                             join t4 in _context.Students on t3.Student equals t4.Id
                             where t1.Id == id
                             select new StudentInTerm
                             {
                                 studentId = t4.Id,
                                 Name = t4.Name,
                                 Gender = t4.Gender,
                             }).ToListAsync();

            return View(data);
        }
    }
}
