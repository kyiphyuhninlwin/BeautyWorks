using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.ProductType;
using Beauty_Works.Repositories.Implementation;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepository productTypeRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductTypeController(IProductTypeRepository productTypeRepository, ICategoryRepository categoryRepository)
        {
            this.productTypeRepository = productTypeRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequestDto request)
        {
            if(request.CategoryID == null)
            {
                return BadRequest("Category ID is required.");
            }

            var category = await categoryRepository.GetByID(request.CategoryID.Value);

            if(category == null)
            {
                return NotFound("Category not found.");
            }

            // Convert Dto to Domain
            var productType = new ProductType
            {
                Name = request.Name,
                CategoryID = request.CategoryID,
                Category = category
            };

            productType = await productTypeRepository.CreateAsync(productType);

            var response = new ProductTypeDto
            {
                ID = productType.ID,
                Name = productType.Name,
                CategoryID = productType.CategoryID,
                CategoryName = productType.Category?.Name
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes([FromQuery] string? sortBy, [FromQuery] string? sortDirection, 
            [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var productTypes = await productTypeRepository.GetAllAsync(sortBy, sortDirection, pageNumber, pageSize);

            var response = new List<ProductTypeDto>();
            foreach(var productType in productTypes)
            {
                response.Add(new ProductTypeDto
                {
                    ID = productType.ID,
                    Name = productType.Name,
                    CategoryID = productType.CategoryID,
                    CategoryName = productType.Category?.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{productTypeID:int}")]
        public async Task<IActionResult> GetProductTypeByID([FromRoute] int productTypeID)
        {
            var productType = await productTypeRepository.GetByID(productTypeID);

            if (productType == null)
            {
                return NotFound();
            }

            var response = new ProductTypeDto
            {
                ID = productType.ID,
                Name = productType.Name,
                CategoryID = productType.CategoryID,
                CategoryName = productType.Category?.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{productTypeID:int}")]
        public async Task<IActionResult> UpdateProductType([FromRoute] int productTypeID, [FromBody] UpdateProductTypeRequestDto request)
        {
            if(request.CategoryID == null)
            {
               return BadRequest("Category ID is required.");
            }

            var category = await categoryRepository.GetByID(request.CategoryID.Value);

            if(category == null)
            {
                return NotFound("Category not found.");
            }

            // Convert Dto to Domain
            var productType = new ProductType
            {
                ID = productTypeID,
                Name = request.Name,
                CategoryID = request.CategoryID
            };

            productType = await productTypeRepository.UpdateAsync(productType);

            if (productType == null)
            {
                return NotFound();
            }

            var response = new ProductTypeDto
            {
                ID = productType.ID,
                Name = productType.Name,
                CategoryID = productType.CategoryID,
                CategoryName = productType.Category?.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{productTypeID:int}")]
        public async Task<IActionResult> DeleteProductType([FromRoute] int productTypeID)
        {
            var productType = await productTypeRepository.DeleteAsync(productTypeID);

            if (productType == null)
            {
                return NotFound();
            }

            var response = new ProductTypeDto
            {
                ID = productType.ID,
                Name = productType.Name,
                CategoryID = productType.CategoryID,
                CategoryName = productType.Category?.Name
            };

            return Ok(response);
        }
    }
}
