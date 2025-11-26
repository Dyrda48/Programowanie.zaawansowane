using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingSession : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Data i czas rozpoczęcia")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Data i czas zakończenia")]
        public DateTime EndTime { get; set; }

        public ICollection<TrainingEntry> TrainingEntries { get; set; } = new List<TrainingEntry>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime < StartTime)
            {
                yield return new ValidationResult(
                    "Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.",
                    new[] { nameof(EndTime) });
            }
        }
    }
}
