using Befit.Data;
using Befit.Models;
using Befit.Models.DTO;
using Befit.Models.DTO.Befit.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Befit.Controllers
{
    [Authorize]
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingSessionsController(ApplicationDbContext context,
                                          UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User);

        // GET: TrainingSessions
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var sessions = await _context.TrainingSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            return View(sessions);
        }

        // GET: TrainingSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var session = await _context.TrainingSessions
                .Include(s => s.TrainingEntries)
                .ThenInclude(e => e.ExerciseType)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
                return NotFound();

            return View(session);
        }

        // GET: TrainingSessions/Create
        public IActionResult Create()
        {
            return View(new TrainingSessionCreateDto());
        }

        // POST: TrainingSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingSessionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var session = new TrainingSession
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                UserId = GetUserId()
            };

            _context.TrainingSessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
                return NotFound();

            var dto = new TrainingSessionCreateDto
            {
                StartTime = session.StartTime,
                EndTime = session.EndTime
            };

            ViewBag.SessionId = id;
            return View(dto);
        }

        // POST: TrainingSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingSessionCreateDto dto)
        {
            var userId = GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.SessionId = id;
                return View(dto);
            }

            session.StartTime = dto.StartTime;
            session.EndTime = dto.EndTime;

            try
            {
                _context.Update(session);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(session.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
                return NotFound();

            return View(session);
        }

        // POST: TrainingSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null)
                return NotFound();

            _context.TrainingSessions.Remove(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}