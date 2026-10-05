using Beauty_Works.Models.Domain;
using Beauty_Works.Models.DTO;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IProductRepository productRepository;

        public ImageController(IImageRepository imageRepository, IProductRepository productRepository) 
        {
            this.imageRepository = imageRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAll();

            var response = new List<ImageDto>();
            foreach (var image in images)
            {
                response.Add(new ImageDto
                {
                    ID = image.ID,
                    Name = image.Name,
                    FileExtension = image.FileExtension,
                    Url = image.Url,
                    Title = image.Title,
                    ProductID = image.Product?.ID,
                    ProductName = image.Product?.Name
                });
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title, [FromForm] int productID)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                // File Upload
                var image = new Image
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Name = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                image = await imageRepository.Upload(file, image);

                await productRepository.AssignImageToProductAsync(productID, image.ID);

                var product = await productRepository.GetByIdAsync(productID);

                // Convert Domain Model To DTO
                var response = new ImageDto
                {
                    ID = image.ID,
                    Name = image.Name,
                    Title = image.Title,
                    FileExtension = image.FileExtension,
                    Url = image.Url,
                    DateCreated = image.DateCreated,
                    ProductID = productID,
                    ProductName = image.Product?.Name
                };

                return Ok(response);
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            // 10mb
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB.");
            }

        }
    }
}
