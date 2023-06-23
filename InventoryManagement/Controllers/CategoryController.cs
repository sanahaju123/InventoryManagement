using InventoryManagement.BusinessLayer.Interfaces;
using InventoryManagement.BusinessLayer.ViewModels;
using InventoryManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpPost]
        [Route("CreateCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel model)
        {
            var categoryExists = await _categoryService.GetCategoryById(model.Id);
            if (categoryExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Category already exists!" });
            Category category = new Category()
            {
                Id = model.Id, Name = model.Name,Description=model.Description,IsDeleted=model.IsDeleted
            };
            var result = await _categoryService.CreateCategory(category);
            if (result == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Category creation failed! Please check details and try again." });

            return Ok(new Response { Status = "Success", Message = "Category created successfully!" });

        }


        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryViewModel model)
        {
            var category = await _categoryService.GetCategoryById(model.Id);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Category With Id = {model.Id} cannot be found" });
            }
            else
            {
                var result = await _categoryService.UpdateCategory(model);
                return Ok(new Response { Status = "Success", Message = "Category updated successfully!" });
            }
        }


        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Category With Id = {id} cannot be found" });
            }
            else
            {
                CategoryViewModel model=new CategoryViewModel();
                model.IsDeleted = true;
                model.Description = category.Description;
                model.Name = category.Name;
                model.Id= id;
                var result = await _categoryService.UpdateCategory(model);
                return Ok(new Response { Status = "Success", Message = "Category deleted successfully!" });
            }
        }


        [HttpGet]
        [Route("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Category With Id = {id} cannot be found" });
            }
            else
            {
                return Ok(category);
            }
        }

        [HttpGet]
        [Route("SearchCategoryByName")]
        public async Task<IActionResult> SearchCategoryByName(int categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);
            if (category == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error", Message = $"Category With Id = {categoryId} cannot be found" });
            }
            else
            {
                return Ok(category);
            }
        }

        [HttpGet]
        [Route("GetAllCategorys")]
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryService.GetAllCategories();
        }
    }
}
