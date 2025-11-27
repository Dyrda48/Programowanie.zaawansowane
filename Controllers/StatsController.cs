using Befit.Data;
using Befit.Models;
using Microsoft.AspNetCore.Mvc;

namespace Befit.Controllers
{
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime fourWeeksAgo = DateTime.Now.AddDays(-28);

            var stats =
                _context.ExerciseTypes
                .Select(type => new ExerciseStatsViewModel
                {
                    ExerciseName = type.Name,

                    TimesPerformed = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.TrainingSession.StartTime >= fourWeeksAgo)
                        .Count(),

                    TotalReps = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.TrainingSession.StartTime >= fourWeeksAgo)
                        .Sum(e => e.Sets * e.Reps),

                    AverageWeight = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.TrainingSession.StartTime >= fourWeeksAgo)
                        .Average(e => (double?)e.Weight) ?? 0,

                    MaxWeight = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.TrainingSession.StartTime >= fourWeeksAgo)
                        .Max(e => (double?)e.Weight) ?? 0
                })
                .ToList();

            return View(stats);
        }
    }
}
