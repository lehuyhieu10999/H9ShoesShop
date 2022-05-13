using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace H9ShoesShopApp.Controllers
{
	public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        

        public ProductController(IProductRepository productRepository,
                                ICategoryRepository categoryRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
           
        }
       
        [HttpGet]
        [Route("Product/{id}/{status}")]
        public JsonResult ChangeStatus(int id, bool status)
        {
            var result = productRepository.ChangeStatus(id, status);
           
            return Json(new { result });
        }
        [HttpGet]
        
        public IActionResult UndoDelete(int id)
        {
            if (productRepository.UndoDelete(id) >0)
            {
                return RedirectToAction("RecycleBin", "Product");
            }
            return RedirectToAction("RecycleBin", "Product");

        }
        public IActionResult Index()
        {
            List<ShowAll> result = (List<ShowAll>)productRepository.showProduct();
            return View(result);
        }
        public IActionResult RecycleBin()
        {
            List<ShowAll> result = (List<ShowAll>)productRepository.showProduct2();
            return View(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = categoryRepository.Gets();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreate model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Categories = categoryRepository.Gets();
                if (productRepository.CreateProduct(model) > 0)
                    return RedirectToAction("ProductbyCategory", "Product", new { categoryid = model.CategoryId });
                else
                    ModelState.AddModelError("", "Tên này đã tồn tại, vui lòng thử lại tên khác!");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult Edit(ProductEdit model)
        {
            if (ModelState.IsValid)
            {
                if (productRepository.Update(model) > 0)
                {
                    return RedirectToAction("ProductbyCategory", "Product", new { categoryid = model.CategoryId });
                }
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var product = productRepository.Get(id);
            if (productRepository.Delete(id))
            {
                return RedirectToAction("ProductbyCategory", "Product", new { categoryid = product.CategoryId });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewBag.Categories = categoryRepository.Gets();
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
        [Route("/Product/ProductbyCategory/{Categoryid}")]
        public IActionResult ProductbyCategory(int Categoryid)
        {
            var result = productRepository.ProductByCategory(Categoryid);
            if (result != null)
            {
                ViewBag.Category = categoryRepository.Get(Categoryid);
                return View(result);
            }
            return View();
        }
    }
}
