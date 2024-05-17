using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using System.Diagnostics;

namespace NCKH_HRM.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly NckhDbContext _context;

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
    }
}
