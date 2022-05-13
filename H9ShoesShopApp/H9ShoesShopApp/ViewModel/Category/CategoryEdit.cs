using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.ViewModel.Category
{
	public class CategoryEdit
    {
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; }
        [Display(Name = "Ảnh")]
        public IFormFile Image { get; set; }
        public bool Status { get; set; }

        public static implicit operator CategoryEdit(Models.Entities.Category v)
        {
            throw new NotImplementedException();
        }
    }
}
