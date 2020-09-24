using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H9ShoesShopApp.Models.Entities
{
    public class OrderDetail
    {
        [Column(Order = 0), Key, ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [Column(Order = 1), Key, ForeignKey("Order")]

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }
    }
}
