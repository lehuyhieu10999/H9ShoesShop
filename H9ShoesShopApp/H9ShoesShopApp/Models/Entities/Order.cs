using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace H9ShoesShopApp.Models.Entities
{
	public class Order
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string CustomerID { get; set; }

        public string CreatedDate { get; set; } 
        [Required] 
        [StringLength(50)]
        public string ShipName { get; set; }
        [Required]
        [StringLength(50)]
        public string ShipPhoneNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        public string ShipEmail { get; set; }

        public bool Status { get; set; }
        public bool IsDelete { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}