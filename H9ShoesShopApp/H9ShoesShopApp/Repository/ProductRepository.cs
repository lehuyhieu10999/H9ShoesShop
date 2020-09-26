using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.ViewModel.Category;
using H9ShoesShopApp.ViewModel.Products;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H9ShoesShopApp.Models.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;
        private readonly ICategoryRepository categoryRepository;
        private IWebHostEnvironment webHostEnvironment;

        public ProductRepository(AppDbContext context,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.categoryRepository = categoryRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
       
      
        public Product ChangeStatus(int id, bool status)
        {
            var product = Get(id);
            product.Status = status;
            context.SaveChanges();
            return product;
        }
        public int UndoDelete(int id)
        {
            var product = Get(id);
            product.IsDelete = false;
            return context.SaveChanges();
        }

        public int CreateProduct(ProductCreate productCreate)
        {
            
            var count = 0;
            foreach (var item in context.Products)
            {
                if (item.ProductName == productCreate.ProductName)
                {
                    count++;
                }
            }
            if (count == 0) {
                var product = new Product()
                {
                    ProductName = productCreate.ProductName,
                    Description = productCreate.Description,
                    Sale = productCreate.Sale,
                    Size = productCreate.Size,
                    Price = productCreate.Price,
                    CategoryId = productCreate.CategoryId,
                    IsDelete = false,
                    Status = false,
                    Brand = productCreate.Brand
                };

                var fileName = string.Empty;
                if (productCreate.Image != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Product");
                    fileName = $"{Guid.NewGuid()}_{productCreate.Image.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        productCreate.Image.CopyTo(fs);
                    }
                }
                product.PathImage = fileName;
                context.Products.Add(product);
                return (context.SaveChanges());
            }
            return 0;
                
        }

        public bool Delete(int id)
        {
            var product = Get(id);
            if (product != null)
            {
                product.IsDelete = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Product Get(int id)
        {
            return context.Products.Find(id);
        }

        public IEnumerable<Product> Gets()
        {
            return context.Products.ToList();
        }
        public object showProduct()
        {
            List<Product> products = Gets().ToList();
            List<Category> categories = categoryRepository.Gets().ToList();
            List<ShowAll> result = (from p in products
                          join c in categories on p.CategoryId equals c.CategoryId
                          where c.IsDelete == false && p.IsDelete == false
                          select (new ShowAll()
                          {
                              ProductId = p.ProductId,
                              ProductName = p.ProductName,
                              Price = p.Price,
                              CategoryName = c.CategoryName,
                              ImagePath = p.PathImage,
                              Status = p.Status,
                              BrandName = p.Brand,
                              IsDelete = p.IsDelete
                          })).ToList();
            return result;
        }
        public object showProduct2()
        {
            List<Product> products = Gets().ToList();
            List<Category> categories = categoryRepository.Gets().ToList();
            List<ShowAll> result = (from p in products
                                    join c in categories on p.CategoryId equals c.CategoryId
                                    where c.IsDelete == false && p.IsDelete == true
                                    select (new ShowAll()
                                    {
                                        ProductId = p.ProductId,
                                        ProductName = p.ProductName,
                                        Price = p.Price,
                                        CategoryName = c.CategoryName,
                                        ImagePath = p.PathImage,
                                        Status = p.Status,
                                        BrandName = p.Brand,
                                        IsDelete = p.IsDelete
                                    })).ToList();
            return result;
        }

        public int Update(ProductEdit productEdit)
        {
            var product = new Product()
            {
                ProductId = productEdit.ProductId,
                ProductName = productEdit.ProductName,
                CategoryId = productEdit.CategoryId,
                Description = productEdit.Description,
                Price = productEdit.Price,
                PathImage = productEdit.ImagePath,
                Sale = productEdit.Sale,
                Size = productEdit.Size,
                Brand = productEdit.Brand,
            };
            product.Status = true;
            var fileName = string.Empty;
            if (productEdit.Image != null)
            {
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Product");
                fileName = $"{Guid.NewGuid()}_{productEdit.Image.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    productEdit.Image.CopyTo(fs);
                }
                product.PathImage = fileName;
                if (!string.IsNullOrEmpty(productEdit.ImagePath))
                {
                    string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                        "images/Product", productEdit.ImagePath);
                    System.IO.File.Delete(delFile);
                }
            }
            if (productEdit.Image == null)
            {
                fileName = productEdit.ImagePath;
            }
            
            var editproduct = context.Products.Attach(product);
            editproduct.State = EntityState.Modified;
            return context.SaveChanges() ;
        }

        public ICollection<Product> ProductByCategory(int categoryid)
        {
            List<Product> products = (from s in context.Products
                                where s.CategoryId == categoryid && !s.IsDelete
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
            return products;
        }
    }
}
