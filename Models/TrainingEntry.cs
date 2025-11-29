using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingEntry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }

        [Required]
        [Display(Name = "Ćwiczenie")]
        public int ExerciseTypeId { get; set; }

        [Required]
        [Range(0.1, 1000)]
        [Display(Name = "Waga (kg)")]
        public double Weight { get; set; }

        [Required]
        [Range(1, 50)]
        [Display(Name = "Serie")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 200)]
        [Display(Name = "Powtórzenia")]
        public int Reps { get; set; }

        [Display(Name = "Sesja treningowa")]
        public TrainingSession TrainingSession { get; set; }

        [Display(Name = "Ćwiczenie")]
        public ExerciseType ExerciseType { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public ApplicationUser User { get; set; }
    }
}