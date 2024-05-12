using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DetailTermsController : Controller
    {
        private readonly NckhDbContext _context;

        public DetailTermsController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DetailTerms
        public async Task<IActionResult> Index(int page = 1)
        {
            var nckhDbContext = _context.DetailTerms.Include(d => d.SemesterNavigation).Include(d => d.TermNavigation);
            int limit = 5;
            var account = await nckhDbContext.OrderBy(c => c.Id).ToPagedListAsync(page, limit);

            return View(account);
        }

        // GET: Admin/DetailTerms/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DetailTerms == null)
            {
                return NotFound();
            }

            var detailTerm = await _context.DetailTerms
                .Include(d => d.SemesterNavigation)
                .Include(d => d.TermNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailTerm == null)
            {
                return NotFound();
            }

            return View(detailTerm);
        }

        // GET: Admin/DetailTerms/Create
        public IActionResult Create()
        {
            ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Name");
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name");
            return View();
        }

        // POST: Admin/DetailTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Term,Semester,StartDate,EndDate,Room,MaxNumber,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] DetailTerm detailTerm)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                detailTerm.CreateBy = admin.Username;
                detailTerm.UpdateBy = admin.Username;
                detailTerm.IsDelete = false;

                _context.Add(detailTerm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Id", detailTerm.Semester);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Id", detailTerm.Term);
            return View(detailTerm);
        }

        // GET: Admin/DetailTerms/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DetailTerms == null)
            {
                return NotFound();
            }

            var detailTerm = await _context.DetailTerms.FindAsync(id);
            if (detailTerm == null)
            {
                return NotFound();
            }
            ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Name", detailTerm.Semester);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Name", detailTerm.Term);
            return View(detailTerm);
        }

        // POST: Admin/DetailTerms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Term,Semester,StartDate,EndDate,Room,MaxNumber,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] DetailTerm detailTerm)
        {
            if (id != detailTerm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    detailTerm.UpdateDate = DateTime.Now;
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    detailTerm.UpdateBy = admin.Username;

                    _context.Update(detailTerm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailTermExists(detailTerm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Semester"] = new SelectList(_context.Semesters, "Id", "Id", detailTerm.Semester);
            ViewData["Term"] = new SelectList(_context.Terms, "Id", "Id", detailTerm.Term);
            return View(detailTerm);
        }

        // GET: Admin/DetailTerms/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            /*if (id == null || _context.DetailTerms == null)
            {
                return NotFound();
            }

            var detailTerm = await _context.DetailTerms
                .Include(d => d.SemesterNavigation)
                .Include(d => d.TermNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detailTerm == null)
            {
                return NotFound();
            }

            return View(detailTerm);*/

            if (_context.DetailTerms == null)
            {
                return Problem("Entity set 'NckhDbContext.DetailTerms'  is null.");
            }
            var detailTerm = await _context.DetailTerms.FindAsync(id);
            if (detailTerm != null)
            {
                _context.DetailTerms.Remove(detailTerm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/DetailTerms/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.DetailTerms == null)
            {
                return Problem("Entity set 'NckhDbContext.DetailTerms'  is null.");
            }
            var detailTerm = await _context.DetailTerms.FindAsync(id);
            if (detailTerm != null)
            {
                _context.DetailTerms.Remove(detailTerm);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool DetailTermExists(long id)
        {
          return (_context.DetailTerms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
