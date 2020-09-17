using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.Diagnostics.Eventing.Reader;
using H9ShoesShopApp.Models.Identities;
using H9ShoesShopApp.Models;
using H9ShoesShopApp.Models.Entities;
using Microsoft.AspNetCore.Builder;

namespace H9ShoesShopApp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext context;
        public const string CartSession = "CartSession";

        public CartController(AppDbContext context)
        {
            this.context = context;
          
        }
        public IActionResult Index()
        {
            return View();
        }
      

    }
}
