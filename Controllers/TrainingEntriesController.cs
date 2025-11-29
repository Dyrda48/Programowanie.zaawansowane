using Befit.Data;
using Befit.Models;
using Befit.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Befit.Controllers
{
    [Authorize]
    public class TrainingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingEntriesController(ApplicationDbContext context,
                                         UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User);

        // GET: TrainingEntries
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var entries = await _context.TrainingEntries
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.TrainingSession.StartTime)
                .ToListAsync();

            return View(entries);
        }

        // GET: TrainingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var entry = await _context.TrainingEntries
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return NotFound();

            return View(entry);
        }

        // GET: TrainingEntries/Create
        public IActionResult Create()
        {
            var userId = GetUserId();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == userId),
                "Id", "StartTime");

            return View(new TrainingEntryCreateDto());
        }

        // POST: TrainingEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingEntryCreateDto dto)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
                ViewData["TrainingSessionId"] = new SelectList(
                    _context.TrainingSessions.Where(s => s.UserId == userId),
                    "Id", "StartTime", dto.TrainingSessionId);

                return View(dto);
            }

            var entry = new TrainingEntry
            {
                TrainingSessionId = dto.TrainingSessionId,
                ExerciseTypeId = dto.ExerciseTypeId,
                Weight = dto.Weight,
                Sets = dto.Sets,
                Reps = dto.Reps,
                UserId = userId
            };

            _context.TrainingEntries.Add(entry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return NotFound();

            var dto = new TrainingEntryCreateDto
            {
                TrainingSessionId = entry.TrainingSessionId,
                ExerciseTypeId = entry.ExerciseTypeId,
                Weight = entry.Weight,
                Sets = entry.Sets,
                Reps = entry.Reps
            };

            ViewBag.EntryId = id;

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == userId),
                "Id", "StartTime", dto.TrainingSessionId);

            return View(dto);
        }

        // POST: TrainingEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingEntryCreateDto dto)
        {
            var userId = GetUserId();

            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.EntryId = id;
                ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", dto.ExerciseTypeId);
                ViewData["TrainingSessionId"] = new SelectList(
                    _context.TrainingSessions.Where(s => s.UserId == userId),
                    "Id", "StartTime", dto.TrainingSessionId);

                return View(dto);
            }

            entry.TrainingSessionId = dto.TrainingSessionId;
            entry.ExerciseTypeId = dto.ExerciseTypeId;
            entry.Weight = dto.Weight;
            entry.Sets = dto.Sets;
            entry.Reps = dto.Reps;

            try
            {
                _context.Update(entry);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var entry = await _context.TrainingEntries
                .Include(e => e.ExerciseType)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return NotFound();

            return View(entry);
        }

        // POST: TrainingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();

            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return NotFound();

            _context.TrainingEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}