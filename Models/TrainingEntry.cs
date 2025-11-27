using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingEntry
    {
        public int Id { get; set; }

        [Display(Name = "trening")]
        public int TrainingSessionId { get; set; }

        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Waga")]
        public double Weight { get; set; }

        [Display(Name = "Ilość setów")]
        public int Sets { get; set; }

        [Display(Name = "Powtórzenia w setu")]
        public int Reps { get; set; }

        public TrainingSession TrainingSession { get; set; }
        public ExerciseType ExerciseType { get; set; }
    }
}