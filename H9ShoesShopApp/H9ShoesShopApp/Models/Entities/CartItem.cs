using System;

namespace H9ShoesShopApp.Models.Entities
{
	[Serializable]
    public class CartItem 
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}