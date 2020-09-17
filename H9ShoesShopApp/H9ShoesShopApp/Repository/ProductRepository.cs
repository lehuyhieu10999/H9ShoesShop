using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.ViewModel.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext context;
        private readonly IRepository<Category> categoryRepository;

        public ProductRepository(AppDbContext context,
            IRepository<Category> categoryRepository)
        {
            this.context = context;
            this.categoryRepository = categoryRepository;
        }
       
      
        public Product ChangeStatus(int id, bool status)
        {
            var product = Get(id);
            product.Status = status;
            return Edit(product);
        }
        public Product Create(Product product)
        {
            var result = context.Products.Add(product);
             context.SaveChanges();
            product.ProductId = result.Entity.ProductId;
            return product;
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

        public Product Edit(Product product)
        {
            var edit = context.Products.Attach(product);
            edit.State = EntityState.Modified;
            context.SaveChanges();
            return product;
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
                              BrandName = p.Brand
                          })).ToList();
            return result;
        } 
    }
}
