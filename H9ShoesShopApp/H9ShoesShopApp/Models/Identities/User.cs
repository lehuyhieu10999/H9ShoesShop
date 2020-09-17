using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Identities
{
    public class User
    {
        public string UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        public string Gender { get; set; }
      
        public string RoleName { get; set; }
    }
}
