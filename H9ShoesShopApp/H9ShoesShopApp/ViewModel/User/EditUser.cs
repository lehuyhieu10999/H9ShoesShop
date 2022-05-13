using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel
{
	public class EditUser
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        public string RoleId { get; set; }
    }
}
