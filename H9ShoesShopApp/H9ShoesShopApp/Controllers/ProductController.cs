using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.ViewModel.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace H9ShoesShopApp.Controllers
{
  public class ProductController : Controller
    {
        private IRepository<Product> productRepository;
        private IRepository<Category> categoryRepository;
        private IWebHostEnvironment webHostEnvironment;

        public ProductController(IRepository<Product> productRepository,
                                IRepository<Category> categoryRepository,
                               IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
       
        [HttpGet]
        [Route("Product/{id}/{status}")]
        public JsonResult ChangeStatus(int id, bool status)
        {
            var result = productRepository.ChangeStatus(id, status);
           
            return Json(new { result });
        }
        public IActionResult Index()
        {
            List<ShowAll> result = (List<ShowAll>)productRepository.showProduct();
            return View(result);
        }
     
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = categoryRepository.Gets().ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreate model)
        {

            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    Sale = model.Sale,
                    Size = model.Size,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    IsDelete = false,
                    Status = false,
                    Brand = model.Brand
                };

                var fileName = string.Empty;
                if (model.Image != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fs);
                    }
                }
                product.PathImage = fileName;
                productRepository.Create(product);
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewBag.Categories = categoryRepository.Gets().ToList();
            try
            {
                var product = productRepository.Get(int.Parse(id));
                if (product != null && !product.IsDelete)
                {
                    var productedit = new ProductEdit()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        CategoryId = product.CategoryId,
                        Description = product.Description,
                        Price = product.Price,
                        ImagePath = product.PathImage,
                        Sale = product.Sale,
                        Size = product.Size,
                        Brand = product.Brand,
                        Status = product.Status
                    };
                    return View(productedit);
                }
                else
                {
                    ViewBag.id = id;
                    return View("~/Views/Error/ProductNotFound.cshtml");
                }
            }
            catch (Exception)
            {
                ViewBag.id = id;
                return View("~/Views/Error/ProductNotFound.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Edit(ProductEdit model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    ProductId = model.ProductId,
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    Price = model.Price,
                    PathImage = model.ImagePath,
                    Sale = model.Sale,
                    Size = model.Size,
                    Brand = model.Brand,
                    Status = model.Status
                };
                var fileName = string.Empty;
                if (model.Image != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fs);
                    }
                    product.PathImage = fileName;
                    if (!string.IsNullOrEmpty(model.ImagePath))
                    {
                        string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                            "images/Product", model.ImagePath);
                        System.IO.File.Delete(delFile);
                    }
                }
                if(model.Image == null)
                {
                    fileName = model.ImagePath;
                }
                var editEmp = productRepository.Edit(product);
                if (editEmp != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            if (productRepository.Delete(id))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
