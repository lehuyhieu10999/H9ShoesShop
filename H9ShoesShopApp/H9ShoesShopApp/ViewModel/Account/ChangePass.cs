using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Account
{
	public class ChangePass
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Bạn chưa Nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Nhập lại mật khẩu mới")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không đúng")]
        public string RepeatNewPassword { get; set; }
    }
}
