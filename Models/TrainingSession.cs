using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rozpoczęcie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Zakończenie treningu")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public ApplicationUser User { get; set; }

        public ICollection<TrainingEntry> TrainingEntries { get; set; } = new List<TrainingEntry>();
    }
}