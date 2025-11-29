using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingEntry
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; }

        [Required]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; }

        [Required]
        [Range(0, 999)]
        [Display(Name = "Obciążenie (kg)")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double Weight { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Serie")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 200)]
        [Display(Name = "Powtórzenia")]
        public int Reps { get; set; }

        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}