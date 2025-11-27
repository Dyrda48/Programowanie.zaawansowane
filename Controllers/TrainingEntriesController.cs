using Befit.Data;
using Befit.Models;
using Befit.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Befit.Controllers
{
    public class TrainingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: TrainingEntries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TrainingEntries
            .Include(t => t.ExerciseType)
            .Include(t => t.TrainingSession)
            .Where(t => t.UserId == GetUserId());
            return View(await applicationDbContext.ToListAsync());
        }
        private string GetUserId()
        {
            return _userManager.GetUserId(User);
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
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetUserId());
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
        public async Task<IActionResult> Create(TrainingEntryCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var entry = new TrainingEntry
                {
                    TrainingSessionId = dto.TrainingSessionId,
                    ExerciseTypeId = dto.ExerciseTypeId,
                    Weight = dto.Weight,
                    Sets = dto.Sets,
                    Reps = dto.Reps,
                    UserId = GetUserId()
                };

                _context.Add(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == GetUserId()),
                "Id", "StartTime", dto.TrainingSessionId);

            return View(dto);
        }

        // GET: TrainingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingEntry = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == GetUserId());

            if (trainingEntry == null)
                return NotFound();

            var dto = new TrainingEntryCreateDto
            {
                TrainingSessionId = trainingEntry.TrainingSessionId,
                ExerciseTypeId = trainingEntry.ExerciseTypeId,
                Weight = trainingEntry.Weight,
                Sets = trainingEntry.Sets,
                Reps = trainingEntry.Reps
            };

            ViewBag.EntryId = id; 
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == GetUserId()),
                "Id", "StartTime", dto.TrainingSessionId);

            return View(dto);
        }

        // POST: TrainingEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingEntryCreateDto dto)
        {
            var existing = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == GetUserId());

            if (existing == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existing.TrainingSessionId = dto.TrainingSessionId;
                existing.ExerciseTypeId = dto.ExerciseTypeId;
                existing.Weight = dto.Weight;
                existing.Sets = dto.Sets;
                existing.Reps = dto.Reps;

                try
                {
                    _context.Update(existing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == GetUserId()),
                "Id", "StartTime", dto.TrainingSessionId);

            return View(dto);
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
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == GetUserId());
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
            var trainingEntry = await _context.TrainingEntries
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (trainingEntry == null)
                return NotFound();

            _context.TrainingEntries.Remove(trainingEntry);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
