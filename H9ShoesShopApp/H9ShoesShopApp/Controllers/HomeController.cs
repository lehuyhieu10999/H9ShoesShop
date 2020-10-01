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
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using H9ShoesShopApp.Models;
using H9ShoesShopApp.Repository;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections;

namespace H9ShoesShopApp.Controllers
{
    public class HomeController : Controller
    {
        private  IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AppDbContext context;



        public HomeController(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
                                IWebHostEnvironment webHostEnvironment,
                               IOrderDetailRepository orderDetailRepository,
                                AppDbContext context)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.orderDetailRepository = orderDetailRepository;
            this.context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("CartSession");

            List<Product> products = productRepository.Gets().Take(20).ToList();
            List<Product> productsale = (from product in productRepository.Gets()
                                         where product.Sale > 0
                                         orderby product.Sale descending
                                         select product).Take(24).ToList();

            var connectionString = "workstation id=h9shoesshop.mssql.somee.com;packet size=4096;user id=lehuyhieu1099_SQLLogin_1;pwd=34t3yt94px;data source=h9shoesshop.mssql.somee.com;persist security info=False;initial catalog=h9shoesshop";
            List<int> eventName = new List<int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               eventName = connection.Query<int>("select p.ProductId from (select Top(10) count(OrderDetails.ProductId) as countProductId, " +
                   "OrderDetails.ProductId as ProductId  from Products inner join OrderDetails on Products.ProductId = OrderDetails.ProductID " +
                   "group by OrderDetails.ProductID " +
                   "order by countProductId desc) as p").ToList();
            
            }
            List<Product> eventNameProduct = new List<Product>();

            foreach(var item in eventName)
            {
                eventNameProduct.Add(productRepository.Get(item));
            }

            var model = new HomeViewModel()
            {
                HomeView = new HomeView
                {
                    products = products,
                    productssale = productsale,
                    bestbuyproducts = eventNameProduct
                }
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

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Search(HomeViewModel model)
        {
            var products = (from p in productRepository.Gets()
                            join c in categoryRepository.Gets() on p.CategoryId equals c.CategoryId
                            where !p.IsDelete && p.Status && c.Status && !c.IsDelete
                            select (new Product()
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                PathImage = p.PathImage,
                                Brand = p.Brand,
                                Size = p.Size,
                                Description = p.Description,
                                Sale = p.Sale,
                                Price = p.Price,
                                CategoryId = p.CategoryId
                            })).ToList();
            List<Product> result = new List<Product>();
            var categories = categoryRepository.Gets();
            if (!String.IsNullOrEmpty(model.Search.SearchString))
            {
                string stringsearch = model.Search.SearchString;
                stringsearch = stringsearch.Trim();
             var a =   Regex.Replace(stringsearch, @"\s+", " ");

                foreach (var item in products)
                {

                    if (Regex.Replace(item.ProductName, @"\s+", " ").ToUpper().Contains(a.ToUpper()))
                        result.Add(item);
                }
            }
            var count = 0;
            if(result.Count == 0 || model.Search.SearchString =="")
            {
                result = products;
                count++;
                
            }
            ViewBag.Searchstring = model.Search.SearchString;
            ViewBag.products = result;
            ViewBag.categories = categories;
            ViewBag.count = count;
            return View();
        }
        [AllowAnonymous]
        [Route("Home/ProductByCategory/{categoryId}/{sort}")]
        public IActionResult ProductByCategory(int categoryId, int sort)
        {
            List<Product> data = new List<Product>();
            switch (sort)
            {
                case 0:
                     data = (from s in context.Products
                                where s.CategoryId == categoryId && !s.IsDelete && s.Status
                                orderby s.CategoryId
                                select (new Product()
                                {
                                    ProductId = s.ProductId,
                                    ProductName = s.ProductName,
                                    Category = s.Category,
                                    Price = s.Price,
                                    CategoryId = s.CategoryId,
                                    IsDelete = s.IsDelete,
                                    Brand = s.Brand,
                                    Description = s.Description,
                                    PathImage = s.PathImage,
                                    Sale = s.Sale,
                                    Size = s.Size,
                                    Status = s.Status
                                })).ToList();
                    return Json(data);
                case 1:
                     data = (from s in context.Products
                                 where s.CategoryId == categoryId && !s.IsDelete && s.Status
                                 orderby s.Price descending
                                 select (new Product()
                                 {
                                     ProductId = s.ProductId,
                                     ProductName = s.ProductName,
                                     Category = s.Category,
                                     Price = s.Price,
                                     CategoryId = s.CategoryId,
                                     IsDelete = s.IsDelete,
                                     Brand = s.Brand,
                                     Description = s.Description,
                                     PathImage = s.PathImage,
                                     Sale = s.Sale,
                                     Size = s.Size,
                                     Status = s.Status
                                 })).ToList();
                    return Json(data);
                case 2:
                     data = (from s in context.Products
                                where s.CategoryId == categoryId && !s.IsDelete && s.Status
                             orderby s.ProductName
                                select (new Product()
                                {
                                    ProductId = s.ProductId,
                                    ProductName = s.ProductName,
                                    Category = s.Category,
                                    Price = s.Price,
                                    CategoryId = s.CategoryId,
                                    IsDelete = s.IsDelete,
                                    Brand = s.Brand,
                                    Description = s.Description,
                                    PathImage = s.PathImage,
                                    Sale = s.Sale,
                                    Size = s.Size,
                                    Status = s.Status
                                })).ToList();
                    return Json(data);
            }
            return Json(new List<Product>());
           
        }


    }
}
