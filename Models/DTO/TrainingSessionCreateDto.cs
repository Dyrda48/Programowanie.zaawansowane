using System;
using System.ComponentModel.DataAnnotations;

namespace Befit.Models.DTO
{
    public class TrainingSessionCreateDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}