using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingEntry
    {
        public int Id { get; set; }

        [Required]
        public int TrainingSessionId { get; set; }
        public TrainingSession? TrainingSession { get; set; }

        [Required]
        public int ExerciseTypeId { get; set; }
        public ExerciseType? ExerciseType { get; set; }

        [Required]
        [Range(0, 1000)]
        public double Weight { get; set; }

        [Required]
        [Range(1, 50)]
        public int Sets { get; set; }

        [Required]
        [Range(1, 100)]
        public int Reps { get; set; }
    }
}