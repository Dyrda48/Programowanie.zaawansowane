using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]

        [Display(Name = "Squats")]
        public string Name { get; set; } = string.Empty;
    }
}