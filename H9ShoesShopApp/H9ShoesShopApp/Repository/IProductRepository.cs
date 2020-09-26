using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.ViewModel.Category;
using H9ShoesShopApp.ViewModel.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H9ShoesShopApp.Models.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> Gets();
        Product Get(int id);
        bool Delete(int id);
        Product ChangeStatus(int id, bool status);
        int UndoDelete(int id);
        object showProduct();
        object showProduct2(); 
        int CreateProduct(ProductCreate productCreate);
        int Update(ProductEdit productEdit);
        ICollection<Product> ProductByCategory(int categoryid);
    }
}
