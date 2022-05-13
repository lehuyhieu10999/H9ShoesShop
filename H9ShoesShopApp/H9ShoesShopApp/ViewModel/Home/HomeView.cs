using H9ShoesShopApp.Models.Entities;
using System.Collections.Generic;

namespace H9ShoesShopApp.ViewModel
{
	public class HomeView 
    {
        public List<Product> products { get; set; }
        public List<Product> productssale { get; set; }
        public Product Product { get; set; }
        public List<Product> bestbuyproducts { get; set; }
    }
}
