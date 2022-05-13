using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Products
{
	public class ProductCreate
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [Display(Name ="Tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Không được để ảnh trống")]
        [Display(Name = "Ảnh")]
        public IFormFile Image { get; set; }
        
        [Required(ErrorMessage = "Nhãn hiệu không được để trống")]
        [Display(Name = "Thương hiệu")]
        public string Brand { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Giá không đúng định dạng")]
        [Display(Name = "Giá")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Size không được để trống")]
        [Display(Name = "Cỡ")]
        public string Size { get; set; }
        [Required(ErrorMessage = "Sale không hợp lệ")]
        [Display(Name = "Mức khuyến mãi")]
        public float Sale { get; set; }
        [Required(ErrorMessage = "Không hợp lệ")]
        [Display(Name = "Loại")]
        public int CategoryId { get; set; }
    }
}
