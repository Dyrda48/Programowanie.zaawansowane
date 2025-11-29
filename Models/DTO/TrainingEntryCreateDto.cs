using System.ComponentModel.DataAnnotations;

namespace Befit.Models.DTO
{
    public class TrainingEntryCreateDto
    {
        [Required]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }

        [Required]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [Required]
        [Range(0, 999)]
        [Display(Name = "Obciążenie (kg)")]
        public double Weight { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Serie")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 200)]
        [Display(Name = "Powtórzenia")]
        public int Reps { get; set; }
    }
}