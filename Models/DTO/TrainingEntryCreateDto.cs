using System.ComponentModel.DataAnnotations;

namespace Befit.Models.DTO
{
    public class TrainingEntryCreateDto
    {
        [Required]
        public int TrainingSessionId { get; set; }

        [Required]
        public int ExerciseTypeId { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public int Sets { get; set; }

        [Required]
        public int Reps { get; set; }
    }
}