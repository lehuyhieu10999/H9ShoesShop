using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Products
{
	public class ShowAll
    {
        public int ProductId { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [Display(Name = "Giá")]
        public float Price { get; set; }
        [Display(Name = "Ảnh")]
        public string ImagePath { get; set; }
        [Display(Name = " Tên loại")]
        public string CategoryName { get; set; }
        [Display(Name = "Thương hiệu")]
        public string BrandName { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
    }
}
