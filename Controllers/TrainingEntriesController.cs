using Befit.Data;
using Befit.Models;
using Befit.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace Befit.Controllers
{
    [Authorize]
    public class TrainingEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingEntriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        // GET: TrainingEntries
        public async Task<IActionResult> Index()
        {
            var entries = _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .Where(t => t.UserId == GetUserId());

            return View(await entries.ToListAsync());
        }

        // GET: TrainingEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var entry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (entry == null)
                return NotFound();

            return View(entry);
        }

        // GET: TrainingEntries/Create
        public IActionResult Create()
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions.Where(s => s.UserId == GetUserId()),
                "Id", "StartTime"
            );
            return View();
        }

        // POST: TrainingEntries/Create
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

            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == GetUserId());

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
                _context.TrainingSessions.Where(s => s.UserId == GetUserId()),
                "Id", "StartTime", dto.TrainingSessionId);

            return View(dto);
        }

        // POST: TrainingEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingEntryCreateDto dto)
        {
            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (entry == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                entry.TrainingSessionId = dto.TrainingSessionId;
                entry.ExerciseTypeId = dto.ExerciseTypeId;
                entry.Weight = dto.Weight;
                entry.Sets = dto.Sets;
                entry.Reps = dto.Reps;

                _context.Update(entry);
                await _context.SaveChangesAsync();

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
                return NotFound();

            var entry = await _context.TrainingEntries
                .Include(t => t.ExerciseType)
                .Include(t => t.TrainingSession)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (entry == null)
                return NotFound();

            return View(entry);
        }

        // POST: TrainingEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entry = await _context.TrainingEntries
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (entry == null)
                return NotFound();

            _context.TrainingEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}