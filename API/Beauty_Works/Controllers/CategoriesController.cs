using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Category;
using Beauty_Works.Repositories.Implementation;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            // Map Dto to Domain Model
            var category = new Category
            {
                Name = request.Name
            };

            await categoryRepository.CreateAsync(category);

            // Map Domain to Dto
            var response = new CategoryDto
            {
                ID = category.ID,
                Name = category.Name
            };

            return Ok(response);
        }

        // Get : /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] string? sortBy, [FromQuery] string? sortDirection, 
            [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var categories = await categoryRepository.GetAllAsync(sortBy, sortDirection, pageNumber, pageSize);

            // Map Domain to Dto
            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto
                {
                    ID = category.ID,
                    Name = category.Name
                });
            }

            return Ok(response);
        }

        // Get : /api/categories/{id}
        [HttpGet]
        [Route("{categoryID:int}")]
        public async Task<IActionResult> GetCategoryByID([FromRoute] int categoryID)
        {
            var existingCategory = await categoryRepository.GetByID(categoryID);

            if (existingCategory == null)
            {
                return NotFound();
            }

            // Map Domain to Dto
            var response = new CategoryDto
            {
                ID = existingCategory.ID,
                Name = existingCategory.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{categoryID:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int categoryID, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                ID = categoryID,
                Name = request.Name
            };

            category = await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            //Convert Domain to Dto
            var response = new CategoryDto
            {
                ID = category.ID,
                Name = category.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{categoryID:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryID)
        {
            var category = await categoryRepository.DeleteAsync(categoryID);

            if (category == null)
            {
                return NotFound();
            }

            // Map Domain to Dto
            var response = new CategoryDto
            {
                ID = category.ID,
                Name = category.Name
            };

            return Ok(response);
        }
    }
}
