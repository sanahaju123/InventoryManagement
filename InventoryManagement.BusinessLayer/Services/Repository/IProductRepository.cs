using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BusinessLayer.Services.Repository
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> GetProductById(int productId);
        Task<Product> SearchProductByName(string name);
        Task<Product> DeleteProductById(int productId);
        Task<Product> UpdateProduct(ProductViewModel model);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
