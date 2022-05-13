using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Products
{
	public class ProductEdit
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string ProductName { get; set; }
        
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Nhãn hiệu không được để trống")]
        public string Brand { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Giá không đúng định dạng")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Size không được để trống")]
        public string Size { get; set; }
        [Required]
        public float Sale { get; set; }
        [Required(ErrorMessage = "Không hợp lệ")]
        public int CategoryId { get; set; }
        public bool Status { get; set; }
    }
}
