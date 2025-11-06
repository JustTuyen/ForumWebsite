using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForumWebsite.Data;
using ForumWebsite.Models.Thread;

namespace ForumWebsite.Controllers.Thread
{
    public class StatusModelsController : Controller
    {
        private readonly MyDBContextApplication _context;

        public StatusModelsController(MyDBContextApplication context)
        {
            _context = context;
        }

        // GET: StatusModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Statuses.ToListAsync());
        }

        // GET: StatusModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusModel = await _context.Statuses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (statusModel == null)
            {
                return NotFound();
            }

            return View(statusModel);
        }

        // GET: StatusModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StatusName,About")] StatusModel statusModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusModel);
        }

        // GET: StatusModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusModel = await _context.Statuses.FindAsync(id);
            if (statusModel == null)
            {
                return NotFound();
            }
            return View(statusModel);
        }

        // POST: StatusModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StatusName,About")] StatusModel statusModel)
        {
            if (id != statusModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusModelExists(statusModel.ID))
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
            return View(statusModel);
        }

        // GET: StatusModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusModel = await _context.Statuses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (statusModel == null)
            {
                return NotFound();
            }

            return View(statusModel);
        }

        // POST: StatusModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusModel = await _context.Statuses.FindAsync(id);
            if (statusModel != null)
            {
                _context.Statuses.Remove(statusModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusModelExists(int id)
        {
            return _context.Statuses.Any(e => e.ID == id);
        }
    }
}
