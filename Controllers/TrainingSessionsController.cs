using Befit.Data;
using Befit.Models;
using Befit.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Befit.Controllers
{
    [Authorize]
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrainingSessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        // GET: TrainingSessions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingSessions
                .Where(t => t.UserId == GetUserId())
                .ToListAsync());
        }

        // GET: TrainingSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingSession = await _context.TrainingSessions
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetUserId());

            if (trainingSession == null)
                return NotFound();

            return View(trainingSession);
        }

        // GET: TrainingSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingSessionCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var session = new TrainingSession
                {
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    UserId = GetUserId()
                };

                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        // GET: TrainingSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingSession = await _context.TrainingSessions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (trainingSession == null)
                return NotFound();

            return View(trainingSession);
        }

        // POST: TrainingSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingSession trainingSession)
        {
            if (id != trainingSession.Id)
                return NotFound();

            var existing = await _context.TrainingSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (existing == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                trainingSession.UserId = existing.UserId;

                try
                {
                    _context.Update(trainingSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingSessionExists(trainingSession.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(trainingSession);
        }

        // GET: TrainingSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingSession = await _context.TrainingSessions
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == GetUserId());

            if (trainingSession == null)
                return NotFound();

            return View(trainingSession);
        }

        // POST: TrainingSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == GetUserId());

            if (trainingSession == null)
                return NotFound();

            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}