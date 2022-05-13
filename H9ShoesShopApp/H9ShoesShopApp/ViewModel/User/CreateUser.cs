using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel
{
	public class CreateUser
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu chưa đúng")]
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPasswork { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [Display(Name = "Ngày sinh")]
        public string DoB { get; set; }
        [Display(Name = "Công ty")]
        public string Company { get; set; }
        
        public string Facebook { get; set; }
        
        public string DTW { get; set; }
        [Display(Name = "Quyền")] 
        public string RoleId { get; set; }
    }
}
