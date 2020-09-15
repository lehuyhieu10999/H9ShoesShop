using H9ShoesShopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext context;
        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Product Create(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public bool Delete(int id)
        {
            var pd = Get(id);
            if (pd != null)
            {
                pd.IsDelete = true;
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
    }
}
