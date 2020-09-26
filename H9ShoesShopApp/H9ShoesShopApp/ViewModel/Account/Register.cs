using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.ViewModel.Account
{
    public class Register
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Giới tính")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [Display(Name = "Ngày sinh")]
        public string DoB { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression("(84|0[3|5|7|8|9])+([0-9]{8})",ErrorMessage ="Vui lòng nhập đúng số điện thoại!")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Cơ quan, công ty")]
        public string Company { get; set; }
        public string Facebook { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile Image { get; set; }
    }
}
