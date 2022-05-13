using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.ViewModel.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace H9ShoesShopApp.Models.Repository
{
	public class CategoryRepository : ICategoryRepository 
    {
        private readonly AppDbContext context;
        private IWebHostEnvironment webHostEnvironment;
        public CategoryRepository(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
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



        public List<Category> Gets()
        {
            return context.Categories.ToList();
        }
        public int UpdateCategory(CategoryEdit categoryEdit)
        {
           
                var category = new Category()
                {
                    CategoryName = categoryEdit.CategoryName,
                    CategoryId = categoryEdit.CategoryId,
                    ImagePath = categoryEdit.ImagePath,
                };
                category.Status = true;
                var fileName = string.Empty;
                if (categoryEdit.Image != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Category");
                    fileName = $"{Guid.NewGuid()}_{categoryEdit.Image.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        categoryEdit.Image.CopyTo(fs);
                    }
                    category.ImagePath = fileName;
                    if (!string.IsNullOrEmpty(categoryEdit.ImagePath))
                    {
                        string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                            "images/Category", categoryEdit.ImagePath);
                        System.IO.File.Delete(delFile);
                    }
                }
                else
                {
                    fileName = categoryEdit.ImagePath;
                }
                category.ImagePath = fileName;
                var editcategory = context.Categories.Attach(category);
                editcategory.State = EntityState.Modified;
                return context.SaveChanges();
          
        }
        public Category ChangeStatus(int id, bool status)
        {
            var category = Get(id);
            category.Status = status;
             Edit(category);
            return category;
        }
        public Category Get(int id)
        {
            return context.Categories.Find(id);
        }


        public int CreateCategory(CategoryCreate categoryCreate)
        {
            var count = 0;
            foreach (var item in context.Categories)
            {
                if (item.CategoryName == categoryCreate.CategoryName)
                {
                    count++;  
                }
            }         
            if (count == 0)
            {
                var category = new Category()
                {
                    CategoryName = categoryCreate.CategoryName,
                    IsDelete = false,
                    Status = true
                };
                var fileName = string.Empty;
                if (categoryCreate.CategoryImage != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/Category");
                    fileName = $"{Guid.NewGuid()}_{categoryCreate.CategoryImage.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        categoryCreate.CategoryImage.CopyTo(fs);
                    }
                }
                if (categoryCreate.CategoryImage == null)
                {
                    fileName = "~/images/Category/nonCat.jpg";
                }
                category.ImagePath = fileName;
                context.Categories.Add(category);
                return context.SaveChanges();
            }
            return 0;
        }

        public int UndoDelete(int id)
        {
            var category = Get(id);
            category.IsDelete = false;
            return context.SaveChanges();
        }
    }
}
