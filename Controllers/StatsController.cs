using Befit.Data;
using Befit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Befit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return _userManager.GetUserId(User);
        }

        public IActionResult Index()
        {
            DateTime monthAgo = DateTime.Now.AddDays(-28);

            var userId = GetUserId();

            var stats = _context.ExerciseTypes
                .Select(type => new ExerciseStatsViewModel
                {
                    ExerciseName = type.Name,

                    TimesPerformed = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= monthAgo)
                        .Count(),

                    TotalReps = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= monthAgo)
                        .Sum(e => (int?)e.Sets * e.Reps) ?? 0,

                    AverageWeight = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= monthAgo)
                        .Average(e => (double?)e.Weight) ?? 0,

                    MaxWeight = _context.TrainingEntries
                        .Where(e => e.ExerciseTypeId == type.Id &&
                                    e.UserId == userId &&
                                    e.TrainingSession.StartTime >= monthAgo)
                        .Max(e => (double?)e.Weight) ?? 0
                })
                .ToList();

            return View(stats);
        }
    }
}