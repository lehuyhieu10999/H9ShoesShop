using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using H9ShoesShopApp.ViewModel;


namespace H9ShoesShopApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

       

        public HomeController(IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
                                IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
     
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
            return View(model);
        }

        public List<Category> GetCategories()
        {
            return categoryRepository.Gets().ToList();
        }

    }
}
