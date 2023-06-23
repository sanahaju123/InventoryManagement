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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            return await _productRepository.CreateProduct(product);
        }

        public async Task<Product> DeleteProductById(int productId)
        {
            return await _productRepository.DeleteProductById(productId);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }

        public async Task<Product> SearchProductByName(string name)
        {
            return await _productRepository.SearchProductByName(name);
        }

        public async Task<Product> UpdateProduct(ProductViewModel model)
        {
            var data=await _productRepository.UpdateProduct(model);
            return data;
        }
    }
}

