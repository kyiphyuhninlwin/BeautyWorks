using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO.Product;
using Beauty_Works.Models.DTO.Variant;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ISubcategoryRepository subcategoryRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IBrandRepository brandRepository;
        private readonly IVariantRepository variantRepository;

        public ProductController(IProductRepository productRepository, ISubcategoryRepository subcategoryRepository,
            IStatusRepository statusRepository, IBrandRepository brandRepository, IVariantRepository variantRepository)
        {
            this.productRepository = productRepository;
            this.subcategoryRepository = subcategoryRepository;
            this.statusRepository = statusRepository;
            this.brandRepository = brandRepository;
            this.variantRepository = variantRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
        {
            if(request.OrderID == null)
            {
                return BadRequest("Order ID is required.");
            }

            if (request.SubcategoryID == null)
            {
                return BadRequest("Subcategory ID is required.");
            }

            if (request.StatusID == null)
            {
                return BadRequest("Status ID is required.");
            }

            if (request.BrandID == null)
            {
                return BadRequest("Brand ID is required.");
            }

            if (request.ImageID == null)
            {
                return BadRequest("Image ID is required.");
            }

            var orderID = await productRepository.GetByOrderIdAsync(request.OrderID.Value);
            var subcategory = await subcategoryRepository.GetByID(request.SubcategoryID.Value);
            var status = await statusRepository.GetByID(request.StatusID.Value);
            var brand = await brandRepository.GetByID(request.BrandID.Value);

            if (orderID != null)
            {
                return BadRequest($"OrderID {request.OrderID} is already taken.");
            }

            // check hasvariant
            var hasVariant = await subcategoryRepository.GetHasVariantAsync(request.SubcategoryID.Value);
            if (hasVariant == null)
            {
                return NotFound($"Subcategory with ID {request.SubcategoryID} not found.");
            }

            var variants = new List<Variant>();
            if (hasVariant == true)
            {
                if (request.VariantNames == null || !request.VariantNames.Any())
                {
                    return BadRequest("At least one variant is required for this product.");
                }

                foreach (var variantName in request.VariantNames)
                {
                    var existing = await variantRepository.GetByNameAsync(variantName);
                    if (existing != null)
                    {
                        variants.Add(existing);
                    }
                    else
                    {
                        var newVariant = new Variant
                        {
                            Name = variantName,
                            SubcategoryID = request.SubcategoryID
                        };
                        await variantRepository.CreateAsync(newVariant);
                        variants.Add(newVariant);
                    }
                }
            }

            // Convert Dto to Domain
            var product = new Product
            {
                Name = request.Name,
                OrderID = request.OrderID,
                Price = request.Price,
                Quantity = request.Quantity,
                Desp = request.Desp,
                PublishedDate = request.PublishedDate,
                ExpiredDate = request.ExpiredDate,
                SubcategoryID = request.SubcategoryID,
                Subcategory = subcategory,
                StatusID = request.StatusID,
                Status = status,
                BrandID = request.BrandID,
                Brand = brand,
                Variants = variants,
            };

            product = await productRepository.CreateAsync(product);

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = product.ID,
                OrderID = product.OrderID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Desp = product.Desp,
                PublishedDate = product.PublishedDate,
                ExpiredDate = product.ExpiredDate,
                SubcategoryID = product.SubcategoryID,
                SubcategoryName = product.Subcategory?.Name,
                StatusID = product.StatusID,
                StatusName = product.Status?.Name,
                BrandID = product.BrandID,
                BrandName = product.Brand?.Name,
                Variants = product.Variants!
                            .Select(v => new VariantDto
                            {
                                ID = v.ID,
                                Name = v.Name,
                                SubcategoryID = v.SubcategoryID,
                                SubcategoryName = v.Subcategory?.Name
                            })
                            .ToList()
            };

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int? subcategoryID, [FromQuery] int? brandID, [FromQuery] int? statusID,
                                                        [FromQuery] int? productTypeID, [FromQuery] string? query,
                                                        [FromQuery] string? sortBy, [FromQuery] string? sortDirection,
                                                        [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 100)
        {
            var products = await productRepository.GetAllAsync(subcategoryID, brandID, statusID, productTypeID, sortBy, sortDirection, query, pageNumber, pageSize);

            // Convert Domain to Dto
            var response = new List<ProductDto>();
            foreach (var product in products)
            {
                response.Add(new ProductDto
                {
                    ID = product.ID,
                    OrderID = product.OrderID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Desp = product.Desp,
                    PublishedDate = product.PublishedDate,
                    ExpiredDate = product.ExpiredDate,
                    SubcategoryID = product.SubcategoryID,
                    SubcategoryName = product.Subcategory?.Name,
                    StatusID = product.StatusID,
                    StatusName = product.Status?.Name,
                    BrandID = product.BrandID,
                    BrandName = product.Brand?.Name,
                    ImageID = product.ImageID,
                    ImageName = product.Image?.Name,
                    Variants = product.Variants != null
                                ? product.Variants.Select(v => new VariantDto
                                {
                                    ID = v.ID,
                                    Name = v.Name,
                                    SubcategoryID = v.SubcategoryID,
                                    SubcategoryName = v.Subcategory?.Name
                                })
                                .ToList() : new List<VariantDto>()
                });
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{productID:int}")]
        public async Task<IActionResult> GetProductByID([FromRoute] int productID)
        {
            var product = await productRepository.GetByIdAsync(productID);
            if (product == null)
            {
                return NotFound();
            }

            //Map Domain to Dto
            var response = new ProductDto
            {
                ID = product.ID,
                OrderID = product.OrderID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Desp = product.Desp,
                PublishedDate = product.PublishedDate,
                ExpiredDate = product.ExpiredDate,
                SubcategoryID = product.SubcategoryID,
                SubcategoryName = product.Subcategory?.Name,
                StatusID = product.StatusID,
                StatusName = product.Status?.Name,
                BrandID = product.BrandID,
                BrandName = product.Brand?.Name,
                ImageID = product.ImageID,
                ImageName = product.Image?.Name,
                Variants = product.Variants!
                            .Select(v => new VariantDto
                            {
                                ID = v.ID,
                                Name = v.Name,
                                SubcategoryID = v.SubcategoryID,
                                SubcategoryName = v.Subcategory?.Name
                            })
                            .ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("display/{orderID:int}")]
        public async Task<IActionResult> GetProductByOrderID([FromRoute] int orderID)
        {
            var product = await productRepository.GetByOrderIdAsync(orderID);
            if (product == null)
            {
                return NotFound();
            }

            //Map Domain to Dto
            var response = new ProductDto
            {
                ID = product.ID,
                OrderID = product.OrderID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Desp = product.Desp,
                PublishedDate = product.PublishedDate,
                ExpiredDate = product.ExpiredDate,
                SubcategoryID = product.SubcategoryID,
                SubcategoryName = product.Subcategory?.Name,
                StatusID = product.StatusID,
                StatusName = product.Status?.Name,
                BrandID = product.BrandID,
                BrandName = product.Brand?.Name,
                ImageID = product.ImageID,
                ImageName = product.Image?.Name,
                Variants = product.Variants!
                            .Select(v => new VariantDto
                            {
                                ID = v.ID,
                                Name = v.Name,
                                SubcategoryID = v.SubcategoryID,
                                SubcategoryName = v.Subcategory?.Name
                            })
                            .ToList()
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{productID:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productID, UpdateProductRequestDto request)
        {
            var existingProduct = await productRepository.GetByIdAsync(productID);

            if (existingProduct == null)
            {
                return NotFound($"Product with ID {productID} not found.");
            }

            if (request.OrderID == null)
            {
                return BadRequest("Order ID is required.");
            }

            if (request.SubcategoryID == null)
            {
                return BadRequest("Subcategory ID is required.");
            }

            if (request.StatusID == null)
            {
                return BadRequest("Status ID is required.");
            }

            if (request.BrandID == null)
            {
                return BadRequest("Brand ID is required.");
            }

            if (request.OrderID != null)
            {
                var checkOrderID = await productRepository.GetByOrderIdAsync(request.OrderID.Value);
                if (checkOrderID != null && checkOrderID.ID != productID)
                {
                    return BadRequest($"OrderID {request.OrderID} is already taken.");
                }
            }
            
            var subcategory = await subcategoryRepository.GetByID(request.SubcategoryID.Value);
            var status = await statusRepository.GetByID(request.StatusID.Value);
            var brand = await brandRepository.GetByID(request.BrandID.Value);

            if (subcategory == null)
            {
                return NotFound($"Subcategory with ID {request.SubcategoryID} not found.");
            }

            if (status == null)
            {
                return NotFound($"Status with ID {request.StatusID} not found.");
            }

            if (brand == null)
            {
                return NotFound($"Brand with ID {request.BrandID} not found.");
            }

            var hasVariant = await subcategoryRepository.GetHasVariantAsync(request.SubcategoryID.Value);
            if (hasVariant == null)
            {
                return NotFound($"Subcategory with ID {request.SubcategoryID} not found.");
            }

            var variants = new List<Variant>();
            if (hasVariant == true)
            {
                if (request.VariantNames == null || !request.VariantNames.Any())
                {
                    return BadRequest("At least one variant is required for this product.");
                }

                foreach (var variantName in request.VariantNames)
                {
                    var existing = await variantRepository.GetByNameAsync(variantName);
                    if (existing != null)
                    {
                        variants.Add(existing);
                    }
                    else
                    {
                        var newVariant = new Variant
                        {
                            Name = variantName,
                            SubcategoryID = request.SubcategoryID
                        };
                        await variantRepository.CreateAsync(newVariant);
                        variants.Add(newVariant);
                    }
                }
            }

            // Convert Dto to Domain
            var product = new Product
            {
                ID = productID,
                OrderID = request.OrderID,
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity,
                Desp = request.Desp,
                PublishedDate = request.PublishedDate,
                ExpiredDate = request.ExpiredDate,
                SubcategoryID = request.SubcategoryID,
                StatusID = request.StatusID,
                BrandID = request.BrandID,
                ImageID = request.ImageID,
                Variants = variants
            };

            var updatedProduct = await productRepository.UpdateAsync(product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = updatedProduct.ID,
                OrderID = updatedProduct.OrderID,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                Quantity = updatedProduct.Quantity,
                Desp = updatedProduct.Desp,
                PublishedDate = updatedProduct.PublishedDate,
                ExpiredDate = updatedProduct.ExpiredDate,
                SubcategoryID = updatedProduct.SubcategoryID,
                SubcategoryName = updatedProduct.Subcategory?.Name,
                StatusID = updatedProduct.StatusID,
                StatusName = updatedProduct.Status?.Name,
                BrandID = updatedProduct.BrandID,
                BrandName = updatedProduct.Brand?.Name,
                ImageID = updatedProduct.ImageID,
                ImageName = updatedProduct.Image?.Name,
                Variants = updatedProduct.Variants!
                            .Select(v => new VariantDto
                            {
                                ID = v.ID,
                                Name = v.Name,
                                SubcategoryID = v.SubcategoryID,
                                SubcategoryName = v.Subcategory?.Name
                            })
                            .ToList()
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{productID:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productID)
        {
            var deletedProduct = await productRepository.DeleteAsync(productID);

            if (deletedProduct == null)
            {
                return NotFound();
            }

            // Convert Domain to Dto
            var response = new ProductDto
            {
                ID = deletedProduct.ID,
                OrderID = deletedProduct.OrderID,
                Name = deletedProduct.Name,
                Price = deletedProduct.Price,
                Quantity = deletedProduct.Quantity,
                Desp = deletedProduct.Desp,
                PublishedDate = deletedProduct.PublishedDate,
                ExpiredDate = deletedProduct.ExpiredDate,
                SubcategoryID = deletedProduct.SubcategoryID,
                SubcategoryName = deletedProduct.Subcategory?.Name,
                StatusID = deletedProduct.StatusID,
                StatusName = deletedProduct.Status?.Name,
                BrandID = deletedProduct.BrandID,
                BrandName = deletedProduct.Brand?.Name,
                ImageID = deletedProduct.ImageID,
                ImageName = deletedProduct.Image?.Name,
                Variants = deletedProduct.Variants!
                            .Select(v => new VariantDto
                            {
                                ID = v.ID,
                                Name = v.Name,
                                SubcategoryID = v.SubcategoryID,
                                SubcategoryName = v.Subcategory?.Name
                            })
                            .ToList()
            };

            return Ok(response);
        }

        [HttpPost("assign-image")]
        public async Task<IActionResult> AssignImageToProduct(int productId, int imageId)
        {
            await productRepository.AssignImageToProductAsync(productId, imageId);
            return Ok("Image assigned to product.");
        }

    }
}
