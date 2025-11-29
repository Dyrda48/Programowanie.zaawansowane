using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Befit.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Imię")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}