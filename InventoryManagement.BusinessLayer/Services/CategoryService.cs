
using InventoryManagement.BusinessLayer.Interfaces;
using InventoryManagement.BusinessLayer.Services.Repository;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            return await _categoryRepository.CreateCategory(category);
        }

        public async Task<Category> DeleteCategoryById(int categoryId)
        {
            return await _categoryRepository.DeleteCategoryById(categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await _categoryRepository.GetCategoryById(categoryId);
        }

        public async Task<Category> SearchCategoryByName(string name)
        {
            return await _categoryRepository.SearchCategoryByName(name);
        }

        public async Task<Category> UpdateCategory(CategoryViewModel model)
        {
            return await _categoryRepository.UpdateCategory(model);
        }
    }
}

       
