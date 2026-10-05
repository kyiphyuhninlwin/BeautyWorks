using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Subcategory;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly IProductTypeRepository productTypeRepository;

        public SubcategoryController(ISubcategoryRepository subcategoryRepository, IProductTypeRepository productTypeRepository)
        {
            this.subcategoryRepository = subcategoryRepository;
            this.productTypeRepository = productTypeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubcategory([FromBody] CreateSubcategoryRequestDto request)
        {
            if(request.ProductTypeID == null)
            {
                return BadRequest("Product Type ID is required.");
            }

            var productType = await productTypeRepository.GetByID(request.ProductTypeID.Value);

            if(productType == null)
            {
                return NotFound("Product Type not found.");
            }

            // Convert Dto to Domain
            var subcategory = new Subcategory
            {
                Name = request.Name,
                HasVariant = request.HasVariant,
                ProductTypeID = request.ProductTypeID,
                ProductType = productType
            };

            subcategory = await subcategoryRepository.CreateAsync(subcategory);

            var response = new SubcategoryDto
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                HasVariant = subcategory.HasVariant,
                ProductTypeID = subcategory.ProductTypeID,
                ProductTypeName = subcategory.ProductType?.Name
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubcategories([FromQuery] string? sortBy, [FromQuery] string? sortDirection, 
            [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var subcategories = await subcategoryRepository.GetAllAsync(sortBy, sortDirection, pageNumber, pageSize);

            var response = new List<SubcategoryDto>();
            foreach (var subcategory in subcategories)
            {
                response.Add(new SubcategoryDto
                {
                    ID = subcategory.ID,
                    Name = subcategory.Name,
                    HasVariant = subcategory.HasVariant,
                    ProductTypeID = subcategory.ProductTypeID,
                    ProductTypeName = subcategory.ProductType?.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{subcategoryID:int}")]
        public async Task<IActionResult> GetSubcategoryByID([FromRoute]int subcategoryID)
        {
           var subcategory = await subcategoryRepository.GetByID(subcategoryID);

            if (subcategory == null)
            {
                return NotFound("Subcategory not found.");
            }

            var response = new SubcategoryDto
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                HasVariant = subcategory.HasVariant,
                ProductTypeID = subcategory.ProductTypeID,
                ProductTypeName = subcategory.ProductType?.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{subcategoryID:int}")]
        public async Task<IActionResult> UpdateSubcategory([FromRoute] int subcategoryID, [FromBody] UpdateSubcategoryRequestDto request)
        {
            if (request.ProductTypeID == null)
            {
                return NotFound("Product Type ID is required.");
            }

            var productType = await productTypeRepository.GetByID(request.ProductTypeID.Value);

            if (productType == null)
            {
                return NotFound("Product Type not found.");
            }

            // Convert Dto to Domain
            var subcategory = new Subcategory
            {
                ID = subcategoryID,
                Name = request.Name,
                HasVariant = request.HasVariant,
                ProductTypeID = request.ProductTypeID
            };

            subcategory = await subcategoryRepository.UpdateAsync(subcategory);

            if (subcategory == null)
            {
                return NotFound("Subcategory not found.");
            }

            var response = new SubcategoryDto
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                HasVariant = subcategory.HasVariant,
                ProductTypeID = subcategory.ProductTypeID,
                ProductTypeName = subcategory.ProductType?.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{subcategoryID:int}")]
        public async Task<IActionResult> DeleteSubcategory([FromRoute] int subcategoryID)
        {
            var subcategory = await subcategoryRepository.DeleteAsync(subcategoryID);

            if (subcategory == null)
            {
                return NotFound("Subcategory not found.");
            }

            var response = new SubcategoryDto
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                HasVariant = subcategory.HasVariant,
                ProductTypeID = subcategory.ProductTypeID,
                ProductTypeName = subcategory.ProductType?.Name
            };

            return Ok(response);
        }
    }
}
