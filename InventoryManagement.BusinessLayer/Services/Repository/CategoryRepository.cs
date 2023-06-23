using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.DataLayer;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Services.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;
        public CategoryRepository(InventoryDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                var result = await _inventoryDbContext.Categories.AddAsync(category);
                await _inventoryDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Category> DeleteCategoryById(int categoryId)
        {
            var category = await _inventoryDbContext.Categories.FindAsync(categoryId);
            try
            {
                category.IsDeleted =true;

                _inventoryDbContext.Categories.Update(category);
                await _inventoryDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                var result = _inventoryDbContext.Categories.
                OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).Take(10).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            try
            {
                return await _inventoryDbContext.Categories.FindAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Category> SearchCategoryByName(string name)
        {
            try
            {
                return await _inventoryDbContext.Categories.FindAsync(name);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Category> UpdateCategory(CategoryViewModel model)
        {
            var category = await _inventoryDbContext.Categories.FindAsync(model.Id);
            try
            {
                category.Id = model.Id;
                category.Name = model.Name;
                category.Description = model.Description;
                category.IsDeleted = model.IsDeleted;

                _inventoryDbContext.Categories.Update(category);
                await _inventoryDbContext.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
