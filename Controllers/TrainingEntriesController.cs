using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Befit.Data;
using Befit.Models;

namespace Befit.Controllers
{
    public class TrainingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingEntries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TrainingEntries.Include(t => t.ExerciseType).Include(t => t.TrainingSession);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TrainingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingEntry == null)
            {
                return NotFound();
            }

            return View(trainingEntry);
        }

        // GET: TrainingEntries/Create
        public IActionResult Create()
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "StartTime");
            return View();
        }

        // POST: TrainingEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrainingSessionId,ExerciseTypeId,Weight,Sets,Reps")] TrainingEntry trainingEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", trainingEntry.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "StartTime", trainingEntry.TrainingSessionId);
            return View(trainingEntry);
        }

        // GET: TrainingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries.FindAsync(id);
            if (trainingEntry == null)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", trainingEntry.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "StartTime", trainingEntry.TrainingSessionId);
            return View(trainingEntry);
        }

        // POST: TrainingEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingSessionId,ExerciseTypeId,Weight,Sets,Reps")] TrainingEntry trainingEntry)
        {
            if (id != trainingEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingEntryExists(trainingEntry.Id))
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
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", trainingEntry.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "StartTime", trainingEntry.TrainingSessionId);
            return View(trainingEntry);
        }

        // GET: TrainingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingEntry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingEntry == null)
            {
                return NotFound();
            }

            return View(trainingEntry);
        }

        // POST: TrainingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingEntry = await _context.TrainingEntries.FindAsync(id);
            if (trainingEntry != null)
            {
                _context.TrainingEntries.Remove(trainingEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingEntryExists(int id)
        {
            return _context.TrainingEntries.Any(e => e.Id == id);
        }
    }
}
