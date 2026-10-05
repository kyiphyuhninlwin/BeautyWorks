using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Subcategory;
using Beauty_Works.Models.DTO.Variant;
using Beauty_Works.Repositories.Implementation;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly IVariantRepository variantRepo;
        private readonly ISubcategoryRepository subcategoryRepo;

        public VariantController(IVariantRepository variantRepo, ISubcategoryRepository subcategoryRepo)
        {
            this.variantRepo = variantRepo;
            this.subcategoryRepo = subcategoryRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(CreateVariantRequestDto request)
        {
            Subcategory? subcategory = null;
            if (request.SubcategoryID.HasValue)
            {
                subcategory = await subcategoryRepo.GetByID(request.SubcategoryID.Value);
                if (subcategory == null)
                {
                    return NotFound("Subcategory not found.");
                }
            }

            var variant = new Variant
            {
                Name = request.Name,
                SubcategoryID = request.SubcategoryID,
                Subcategory = subcategory
            };

            await variantRepo.CreateAsync(variant);

            var response = new VariantDto
            {
                ID = variant.ID,
                Name = variant.Name,
                SubcategoryID = variant.SubcategoryID,
                SubcategoryName = variant.Subcategory?.Name
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVariant([FromQuery] string? sortBy, [FromQuery] string? sortDirection, 
            [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var variants = await variantRepo.GetAllAsync(sortBy, sortDirection, pageNumber, pageSize);

            // Map Domain to Dto
            var response = new List<VariantDto>();
            foreach (var variant in variants)
            {
                response.Add(new VariantDto
                {
                    ID = variant.ID,
                    Name = variant.Name,
                    SubcategoryID = variant.SubcategoryID,
                    SubcategoryName = variant.Subcategory?.Name
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{variantID:int}")]
        public async Task<IActionResult> GetVariantByID([FromRoute] int variantID)
        {
            var existingVariant = await variantRepo.GetByID(variantID);

            if (existingVariant == null)
            {
                return NotFound();
            }

            // Map Domain to Dto
            var response = new VariantDto
            {
                ID = existingVariant.ID,
                Name = existingVariant.Name,
                SubcategoryID = existingVariant.SubcategoryID,
                SubcategoryName = existingVariant.Subcategory?.Name
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{variantID:int}")]
        public async Task<IActionResult> UpdateVariant([FromRoute] int variantID, [FromBody] UpdateVariantRequestDto request)
        {
            if (request.SubcategoryID == null)
            {
                return BadRequest("Subcategory ID is required.");
            }

            var subcategory = await subcategoryRepo.GetByID(request.SubcategoryID.Value);

            if (subcategory == null)
            {
                return NotFound("Subcategory not found.");
            }

            var variant = new Variant
            {
                ID = variantID,
                Name = request.Name,
                SubcategoryID = request.SubcategoryID
            };

            variant = await variantRepo.UpdateAsync(variant);

            if (variant == null)
            {
                return NotFound();
            }

            // Map Domain to dto
            var response = new VariantDto
            {
                ID = variantID,
                Name = variant.Name,
                SubcategoryID = variant.SubcategoryID,
                SubcategoryName = variant.Subcategory?.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{variantID:int}")]
        public async Task<IActionResult> DeleteVariant([FromRoute] int variantID)
        {
            var variant = await variantRepo.DeleteAsync(variantID);

            if (variant == null)
            {
                return NotFound();
            }

            var response = new VariantDto
            {
                ID = variant.ID,
                Name = variant.Name,
                SubcategoryID = variant.SubcategoryID,
                SubcategoryName = variant.Subcategory?.Name
            };

            return Ok(response);
        }
    }
}
