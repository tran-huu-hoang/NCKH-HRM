using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NCKH_HRM.Models;
using X.PagedList;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class YearsController : BaseController
    {
        private readonly NckhDbContext _context;

        public YearsController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Years
        public async Task<IActionResult> Index(int name, int page = 1)
        {
            //số bản ghi trên 1 trang
            int limit = 5;

            var account = await _context.Years.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            /*if (!(name == null))
            {
                account = await _context.Years.Where(c => c.Name == name).OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            }*/

            ViewBag.keyword = name;
            return View(account);
        }

        // GET: Admin/Years/Details/5
        /*public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Years == null)
            {
                return NotFound();
            }

            var year = await _context.Years
                .FirstOrDefaultAsync(m => m.Id == id);
            if (year == null)
            {
                return NotFound();
            }

            return View(year);
        }*/

        // GET: Admin/Years/Create
        public IActionResult Create()
        {
            return View();
        }

         //POST: Admin/Years/Create
         //To protect from overposting attacks, enable the specific properties you want to bind to.
         //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Year year)
        {
            if (ModelState.IsValid)
            {
                _context.Add(year);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(year);
        }

        // GET: Admin/Years/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Years == null)
            {
                return NotFound();
            }

            var year = await _context.Years.FindAsync(id);
            if (year == null)
            {
                return NotFound();
            }
            return View(year);
        }

        // POST: Admin/Years/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Year year)
        {
            if (id != year.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(year);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearExists(year.Id))
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
            return View(year);
        }

        // GET: Admin/Years/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            /*if (id == null || _context.Years == null)
            {
                return NotFound();
            }

            var year = await _context.Years
                .FirstOrDefaultAsync(m => m.Id == id);
            if (year == null)
            {
                return NotFound();
            }

            return View(year);*/

            if (_context.Years == null)
            {
                return Problem("Entity set 'NckhDbContext.Years'  is null.");
            }
            var year = await _context.Years.FindAsync(id);
            if (year != null)
            {
                _context.Years.Remove(year);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Years/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Years == null)
            {
                return Problem("Entity set 'NckhDbContext.Years'  is null.");
            }
            var year = await _context.Years.FindAsync(id);
            if (year != null)
            {
                _context.Years.Remove(year);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool YearExists(long id)
        {
          return (_context.Years?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
