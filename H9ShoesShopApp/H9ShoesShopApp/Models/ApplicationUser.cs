using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.Models
{
	public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Gender { get; set; }

        public string DoB { get; set; }

        public string Company { get; set; }

        public string Facebook { get; set; }

        public string DTW { get; set; }
        public string ImagePath { get; set; }
    }
}
