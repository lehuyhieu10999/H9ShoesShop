#region using

// System
using System;

// ASP .NET Core
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

// H9
using H9ShoesShopApp.Models.Repository;
using H9ShoesShopApp.ViewModel.Category;

#endregion

namespace H9ShoesShopApp.Controllers
{

	public class CategoryController : Controller
    {
        #region Biến

        /// <summary>
        /// WebHostEnvironment
        /// </summary>
        private readonly IWebHostEnvironment webHostEnvironment;

        /// <summary>
        /// Repository: Category
        /// </summary>
        private readonly ICategoryRepository categoryRepository;

        /// <summary>
        /// Repository: Product
        /// </summary>
        public readonly IProductRepository productRepository;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="productRepository"></param>
        /// <param name="categoryRepository"></param>
        public CategoryController(IWebHostEnvironment webHostEnvironment,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
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
        public IActionResult RecycleBin()
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
                if (categoryRepository.CreateCategory(model) > 0)
                {
                    ViewBag.Categories = categoryRepository.Gets();
                    return RedirectToAction("Index", "Category");
                }
                else  ModelState.AddModelError("", "Tên này đã tồn tại, vui lòng chọn tên khác");

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
                if (categoryRepository.UpdateCategory(model) > 0)
                {
                    return RedirectToAction("Index", "Category");
                }
                else ModelState.AddModelError("", "Tên này đã tồn tại, vui lòng chọn tên khác");
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
        public IActionResult UndoDelete(int id)
        {
            if (categoryRepository.UndoDelete(id) > 0)
            {
                return RedirectToAction("RecycleBin", "Category");
            }
            return View();

        }
    }
}