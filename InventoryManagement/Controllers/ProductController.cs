
using InventoryManagement.BusinessLayer.Interfaces;
using InventoryManagement.BusinessLayer.Services;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProduct([FromBody] ProductViewModel model)
        {
            var productExists = await _productService.GetProductById(model.Id);
            if (productExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Product already exists!" });
            Product product = new Product()
            {
                Id = model.Id,
                Name = model.Name,
                CategoryId=model.CategoryId,
                IsDeleted = model.IsDeleted,
                price=model.price
            };
            var result = await _productService.CreateProduct(product);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Product creation failed! Please check details and try again." });

            return Ok(new Response { Status = "Success", Message = "Product created successfully!" });

        }


        /// <summary>
        /// To update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductViewModel model)
        {
            var product = await _productService.GetProductById(model.Id);
            if (product == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Product With Id = {model.Id} cannot be found" });
            }
            else
            {
                var result = await _productService.UpdateProduct(model);
                return Ok(new Response { Status = "Success", Message = "Product updated successfully!" });
            }
        }

        /// <summary>
        /// To delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Product With Id = {id} cannot be found" });
            }
            else
            {
                ProductViewModel model = new ProductViewModel();
                model.IsDeleted = true;
                model.price = product.price;
                model.Name = product.Name;
                model.Id = product.Id;
                model.CategoryId = product.CategoryId;
                var result = await _productService.UpdateProduct(model);
                return Ok(new Response { Status = "Success", Message = "Product deleted successfully!" });
            }
        }

        /// <summary>
        /// To get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Product With Id = {id} cannot be found" });
            }
            else
            {
                return Ok(product);
            }
        }

        /// <summary>
        /// Search product by name
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchProductByName")]
        public async Task<IActionResult> SearchProductByName(int productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Product With Id = {productId} cannot be found" });
            }
            else
            {
                return Ok(product);
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productService.GetAllProducts();
        }
    }
}
