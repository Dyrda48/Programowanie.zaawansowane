using Befit.Data;
using Befit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Befit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatsController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId() => _userManager.GetUserId(User);

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var fromDate = DateTime.Now.AddDays(-28);

            var stats = await _context.ExerciseTypes
                .Select(type => new ExerciseStatsViewModel
                {
                    ExerciseName = type.Name,

                    TimesPerformed = _context.TrainingEntries
                        .Include(e => e.TrainingSession)
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= fromDate)
                        .Count(),

                    TotalReps = _context.TrainingEntries
                        .Include(e => e.TrainingSession)
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= fromDate)
                        .Sum(e => (int?)(e.Sets * e.Reps)) ?? 0,

                    AverageWeight = _context.TrainingEntries
                        .Include(e => e.TrainingSession)
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= fromDate)
                        .Average(e => (double?)e.Weight) ?? 0,

                    MaxWeight = _context.TrainingEntries
                        .Include(e => e.TrainingSession)
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= fromDate)
                        .Max(e => (double?)e.Weight) ?? 0
                })
                .ToListAsync();

            return View(stats);
        }
    }
}