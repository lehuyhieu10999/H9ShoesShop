using H9ShoesShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.ViewModel.Order
{
    public class ShowOrderDetail
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string CustomerID { get; set; }

        public string CreatedDate { get; set; }

        public string ShipName { get; set; }

        public string ShipPhoneNumber { get; set; }

        public string ShipAddress { get; set; }
 
        public string ShipEmail { get; set; }
        public int Quantity { get; set; }

        public float Price { get; set; }

        public bool Status { get; set; }
    }
}
