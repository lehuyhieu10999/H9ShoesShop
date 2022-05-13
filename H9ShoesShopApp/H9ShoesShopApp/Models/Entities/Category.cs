using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.Models.Entities
{
	public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public ICollection<Product> Product { get; set; }
        public bool IsDelete { get; set; }
        public bool Status { get; set; }
    }
}