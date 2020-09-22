using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
