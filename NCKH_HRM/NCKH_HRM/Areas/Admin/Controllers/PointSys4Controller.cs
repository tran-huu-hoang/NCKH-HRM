using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using Newtonsoft.Json;
using X.PagedList;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PointSys4Controller : BaseController
    {
        private readonly NckhDbContext _context;

        public PointSys4Controller(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PointSys4
        public async Task<IActionResult> Index(int page = 1)
        {
            int limit = 5;

            var pointSys4 = await _context.PointSys4s.OrderBy(c => c.Id).ToPagedListAsync(page, limit);

            return View(pointSys4);
        }

        // GET: Admin/PointSys4/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PointSys4s == null)
            {
                return NotFound();
            }

            var pointSys4 = await _context.PointSys4s
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointSys4 == null)
            {
                return NotFound();
            }

            return View(pointSys4);
        }

        // GET: Admin/PointSys4/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PointSys4/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Range1,Range2,Point,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] PointSys4 pointSys4)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                pointSys4.CreateBy = admin.Username;
                pointSys4.UpdateBy = admin.Username;
                pointSys4.IsDelete = false;
                
                _context.Add(pointSys4);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pointSys4);
        }

        // GET: Admin/PointSys4/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PointSys4s == null)
            {
                return NotFound();
            }

            var pointSys4 = await _context.PointSys4s.FindAsync(id);
            if (pointSys4 == null)
            {
                return NotFound();
            }
            ViewBag.Range1 = pointSys4.Range1;
            ViewBag.Range2 = pointSys4.Range2;
            ViewBag.Point = pointSys4.Point;
            return View(pointSys4);
        }

        // POST: Admin/PointSys4/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Range1,Range2,Point,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] PointSys4 pointSys4)
        {
            if (id != pointSys4.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    pointSys4.UpdateBy = user.Username;
                    pointSys4.UpdateDate = DateTime.Now;

                    _context.Update(pointSys4);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PointSys4Exists(pointSys4.Id))
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
            return View(pointSys4);
        }

        // GET: Admin/PointSys4/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*if (id == null || _context.PointSys4s == null)
            {
                return NotFound();
            }

            var pointSys4 = await _context.PointSys4s
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pointSys4 == null)
            {
                return NotFound();
            }

            return View(pointSys4);*/
            if (_context.PointSys4s == null)
            {
                return Problem("Entity set 'NckhDbContext.PointSys4s'  is null.");
            }
            var pointSys4 = await _context.PointSys4s.FindAsync(id);
            if (pointSys4 != null)
            {
                _context.PointSys4s.Remove(pointSys4);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/PointSys4/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PointSys4s == null)
            {
                return Problem("Entity set 'NckhDbContext.PointSys4s'  is null.");
            }
            var pointSys4 = await _context.PointSys4s.FindAsync(id);
            if (pointSys4 != null)
            {
                _context.PointSys4s.Remove(pointSys4);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool PointSys4Exists(int id)
        {
          return (_context.PointSys4s?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
