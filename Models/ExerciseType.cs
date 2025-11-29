using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Name { get; set; }
    }
}