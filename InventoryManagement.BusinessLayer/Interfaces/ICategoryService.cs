
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(Category category);
        Task<Category> GetCategoryById(int categoryId);
        Task<Category> SearchCategoryByName(string name);
        Task<Category> DeleteCategoryById(int categoryId);
        Task<Category> UpdateCategory(CategoryViewModel model);
        Task<IEnumerable<Category>> GetAllCategories();
    }
}
