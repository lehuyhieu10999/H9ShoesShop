using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.ViewModel.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace H9ShoesShopApp.Controllers
{
   
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IRepository<Category> categoryRepository;
        public readonly IRepository<Product> productRepository;

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
            var Categories = categoryRepository.Gets();
            return View(Categories);
        }
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryCreate model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    CategoryName = model.CategoryName,
                    IsDelete = false,
                    Status = true
                };
                var fileName = string.Empty;
                if (model.CategoryImage != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Category");
                    fileName = $"{Guid.NewGuid()}_{model.CategoryImage.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.CategoryImage.CopyTo(fs);
                    }
                }
                if(model.CategoryImage == null)
                {
                    fileName = "~/images/Category/nonCat.jpg";
                }
                category.ImagePath = fileName;
                categoryRepository.Create(category);
                ViewBag.Categories = categoryRepository.Gets();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Categories = categoryRepository.Gets();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                var category = categoryRepository.Get(int.Parse(id));
                if (category != null && !category.IsDelete)
                {
                    var editcategory = new CategoryEdit()
                    {
                        ImagePath = category.ImagePath,
                        CategoryName = category.CategoryName,
                        CategoryId = category.CategoryId,
                        Status = category.Status
                    };
                    return View(editcategory);
                }
                else
                {
                    ViewBag.id = id;
                    return View("~/Views/Error/CategoryNotFound.cshtml");
                }
            }
            catch (Exception)
            {
                ViewBag.id = id;
                return View("~/Views/Error/CategoryNotFound.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Edit(CategoryEdit model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    CategoryName = model.CategoryName,
                    CategoryId = model.CategoryId,
                    ImagePath = model.ImagePath,
                    Status = model.Status
                };
                var fileName = string.Empty;
                if (model.Image != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Category");
                    fileName = $"{Guid.NewGuid()}_{model.Image.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fs);
                    }
                    category.ImagePath = fileName;
                    if (!string.IsNullOrEmpty(model.ImagePath))
                    {
                        string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                            "images/Category", model.ImagePath);
                        System.IO.File.Delete(delFile);
                    }
                }
                else
                {
                    fileName = model.ImagePath;
                }
                category.ImagePath = fileName;
                var editEmp = categoryRepository.Edit(category);
                if (editEmp != null)
                {
                    return RedirectToAction("Index", "Category");
                }
            }
            return View();
        }
        public IActionResult Delete(int id)
        {

            if (categoryRepository.Delete(id))
            {
                return RedirectToAction("Index","Category");
            }
            return View();
        }
        [HttpGet]
        [Route("Category/{id}/{status}")]
        public JsonResult ChangeStatus(int id, bool status)
        {
            var result = categoryRepository.ChangeStatus(id, status);
            return Json(new { result });
        }
    }
}
