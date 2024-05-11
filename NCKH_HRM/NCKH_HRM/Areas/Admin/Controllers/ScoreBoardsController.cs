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
    public class ScoreBoardsController : BaseController
    {
        private readonly NckhDbContext _context;

        public ScoreBoardsController(NckhDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ScoreBoards
        public async Task<IActionResult> Index(int page = 1)
        {
            //số bản ghi trên 1 trang
            int limit = 5;

            var account = await _context.ScoreBoards.OrderBy(c => c.Id).ToPagedListAsync(page, limit);
            return View(account);
        }

        // GET: Admin/ScoreBoards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScoreBoards == null)
            {
                return NotFound();
            }

            var scoreBoard = await _context.ScoreBoards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scoreBoard == null)
            {
                return NotFound();
            }

            return View(scoreBoard);
        }

        // GET: Admin/ScoreBoards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ScoreBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] ScoreBoard scoreBoard)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                scoreBoard.CreateBy = admin.Username;
                scoreBoard.UpdateBy = admin.Username;
                scoreBoard.IsDelete = false;

                _context.Add(scoreBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scoreBoard);
        }

        // GET: Admin/ScoreBoards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScoreBoards == null)
            {
                return NotFound();
            }

            var scoreBoard = await _context.ScoreBoards.FindAsync(id);
            if (scoreBoard == null)
            {
                return NotFound();
            }
            return View(scoreBoard);
        }

        // POST: Admin/ScoreBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,CreateBy,UpdateBy,CreateDate,UpdateDate,IsDelete,IsActive")] ScoreBoard scoreBoard)
        {
            if (id != scoreBoard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    scoreBoard.UpdateDate = DateTime.Now;
                    var admin = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("AdminLogin"));
                    scoreBoard.UpdateBy = admin.Username;

                    _context.Update(scoreBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreBoardExists(scoreBoard.Id))
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
            return View(scoreBoard);
        }

        // GET: Admin/ScoreBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*if (id == null || _context.ScoreBoards == null)
            {
                return NotFound();
            }

            var scoreBoard = await _context.ScoreBoards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scoreBoard == null)
            {
                return NotFound();
            }

            return View(scoreBoard);*/

            if (_context.ScoreBoards == null)
            {
                return Problem("Entity set 'NckhDbContext.ScoreBoards'  is null.");
            }
            var scoreBoard = await _context.ScoreBoards.FindAsync(id);
            if (scoreBoard != null)
            {
                _context.ScoreBoards.Remove(scoreBoard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ScoreBoards/Delete/5
        /*[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScoreBoards == null)
            {
                return Problem("Entity set 'NckhDbContext.ScoreBoards'  is null.");
            }
            var scoreBoard = await _context.ScoreBoards.FindAsync(id);
            if (scoreBoard != null)
            {
                _context.ScoreBoards.Remove(scoreBoard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool ScoreBoardExists(int id)
        {
          return (_context.ScoreBoards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
