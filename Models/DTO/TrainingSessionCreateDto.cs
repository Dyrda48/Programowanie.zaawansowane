using System;
using System.ComponentModel.DataAnnotations;

namespace Befit.Models.DTO
{
    using System.ComponentModel.DataAnnotations;

    namespace Befit.Models.DTO
    {
        public class TrainingSessionCreateDto
        {
            [Required]
            [Display(Name = "Data rozpoczęcia")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
            public DateTime StartTime { get; set; }

            [Required]
            [Display(Name = "Data zakończenia")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
            public DateTime EndTime { get; set; }
        }
    }
}