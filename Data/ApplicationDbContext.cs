using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Befit.Models;

namespace Befit.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ExerciseType> ExerciseTypes { get; set; } = default!;
        public DbSet<TrainingSession> TrainingSessions { get; set; } = default!;
        public DbSet<TrainingEntry> TrainingEntries { get; set; } = default!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}