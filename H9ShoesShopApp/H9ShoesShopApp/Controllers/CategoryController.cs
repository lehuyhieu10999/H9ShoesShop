using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace H9ShoesShopApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IRepository<Category> categoryRepository;
        public IRepository<Product> productRepository { get; }

        public CategoryController(IWebHostEnvironment webHostEnvironment,
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var Categories = categoryRepository.Gets().ToList();
            return View(Categories);
        }
        public ViewResult Create()
        {
            return View();
        }
        

    }
}
