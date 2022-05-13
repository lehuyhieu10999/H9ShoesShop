using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.Models.Entities
{
	public class Product
	{
		public int ProductId { get; set; }
		[Required]
		public string ProductName { get; set; }
		[Required]
		public string PathImage { get; set; }
		[Required]
		public string Brand { get; set; }
		[Required]
		public string Size { get; set; }
		public string Description { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		[Required]
		public float Sale { get; set; }
		[Required]
		public float Price { get; set; }
		public bool IsDelete { get; set; }
		public bool Status { get; set; }

	}
}