using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Account
{
	public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        public bool Rememberme { get; set; }
        public string returnUrl { get; set; }
    }
}
