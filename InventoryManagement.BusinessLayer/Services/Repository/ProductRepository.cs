
using InventoryManagement.BusinessLayer.Services.Repository;
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
    public class ProductRepository:IProductRepository
    {
        private readonly InventoryDbContext _inventoryDbContext;
        public ProductRepository(InventoryDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                var result = await _inventoryDbContext.Products.AddAsync(product);
                await _inventoryDbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Product> DeleteProductById(int productId)
        {
            var product = await _inventoryDbContext.Products.FindAsync(productId);
            try
            {
                product.IsDeleted = true;

                _inventoryDbContext.Products.Update(product);
                await _inventoryDbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                var result = _inventoryDbContext.Products.
                OrderByDescending(x => x.Id).Where(x => x.IsDeleted == false).Take(10).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                return await _inventoryDbContext.Products.FindAsync(productId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Product> SearchProductByName(string name)
        {
            try
            {
                return await _inventoryDbContext.Products.FindAsync(name);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<Product> UpdateProduct(ProductViewModel model)
        {
            var product = await _inventoryDbContext.Products.FindAsync(model.Id);
            try
            {
                product.Id = model.Id;
                product.Name = model.Name;
                product.price = model.price;
                product.IsDeleted = model.IsDeleted;
                product.CategoryId = model.CategoryId;
                _inventoryDbContext.Products.Update(product);
                await _inventoryDbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
