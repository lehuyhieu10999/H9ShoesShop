using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Category
{
	public class CategoryCreate
    {
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
        [Display(Name = "Ảnh")]
        public IFormFile CategoryImage { get; set; }

    }
}
