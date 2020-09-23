using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using H9ShoesShopApp.ViewModel;
using System;
using H9ShoesShopApp.Helpers;
using Microsoft.AspNetCore.Http;

namespace H9ShoesShopApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

       

        public HomeController(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
                                IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
          var cart =  HttpContext.Session.GetObjectFromJson<List<CartItem>>("CartSession");

            List<Product> products = productRepository.Gets().Take(20).ToList();
            List<Product> productsale = (from product in productRepository.Gets() 
                                         where product.Sale > 0 
                                         select product).TakeLast(10).ToList();
            var model = new HomeView()
            {
               products = products,
               productssale = productsale
            };
            ViewBag.Categories = GetCategories();
            ViewBag.cart = cart;
            return View(model);
        }

        public List<Category> GetCategories()
        {
            return categoryRepository.Gets().ToList();
        }
        [AllowAnonymous]
        public IActionResult Cart(int id)
        {
            var product = productRepository.Get(id);
            var category = categoryRepository.Get(product.CategoryId);
            var categoryid = product.CategoryId;
            List<Product> products = (from p in productRepository.Gets() where p.CategoryId == p.CategoryId select p).Take(10).ToList();
            List<Category> categories = GetCategories();
            ViewBag.Product = product;
            ViewBag.Products = products;
            ViewBag.Categories = categories;
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.categoryid = categoryid;
            return View();
        }
        
        [HttpPost]
        [Route("Home/Search/{searchString}")]
        public ActionResult Search(string searchString)
        {
            var products = from p in productRepository.Gets()
                        join c in categoryRepository.Gets() on p.CategoryId equals c.CategoryId
                        where !p.IsDelete && p.Status && c.Status && !c.Status 
                        select new { p.ProductId, p.ProductName, p.PathImage, p.Brand, p.Size, p.Description, p.Sale,p.Price,p.CategoryId};

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }
            return View(products);
        }



    }
}
