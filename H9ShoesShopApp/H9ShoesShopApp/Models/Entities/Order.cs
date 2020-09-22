using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Entities
{
    public class Order
    {
        
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string CustomerID { get; set; }

        public string CreatedDate { get; set; } 
      
        [StringLength(50)]
        public string ShipName { get; set; }

        [StringLength(50)]
        public string ShipPhoneNumber { get; set; }

        [StringLength(50)]
        public string ShipAddress { get; set; }

        [StringLength(50)]
        public string ShipEmail { get; set; }

        public bool Status { get; set; }
        public bool IsDelete { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

      
    }
}
