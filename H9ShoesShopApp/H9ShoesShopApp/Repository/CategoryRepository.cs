using H9ShoesShopApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly AppDbContext context;
        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Category ChangeStatus(int id, bool status)
        {
            var category = Get(id);
            category.Status = status;
            return Edit(category);
        }

        public Category Create(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return category;
        }

        public bool Delete(int id)
        {
            var category = Get(id);
            if (category != null)
            {
                category.IsDelete = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Category Edit(Category category)
        {
            var edit = context.Categories.Attach(category);
            edit.State = EntityState.Modified;
            context.SaveChanges();
            return category;
        }

        public Category Get(int id)
        {
            return context.Categories.Find(id);
        }

        public IEnumerable<Category> Gets()
        {
            return context.Categories.ToList();
        }

        public object showProduct()
        {
            throw new NotImplementedException();
        }
    }
}
