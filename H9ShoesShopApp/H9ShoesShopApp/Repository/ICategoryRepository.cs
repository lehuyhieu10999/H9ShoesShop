using H9ShoesShopApp.Models.Entities;
using H9ShoesShopApp.ViewModel.Category;
using System.Collections.Generic;

namespace H9ShoesShopApp.Models.Repository
{
	public interface ICategoryRepository
    {
        List<Category> Gets();
        Category Get(int id);
        bool Delete(int id);
        int CreateCategory(CategoryCreate categoryCreate);
        int UpdateCategory(CategoryEdit categoryEdit);
        Category ChangeStatus(int id, bool status);
        int UndoDelete(int id);

    }
}
