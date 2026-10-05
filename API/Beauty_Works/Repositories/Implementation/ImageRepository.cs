using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Image>> GetAll()
        {
            return await dbContext.Images.Include(i => i.Product).ToListAsync();
        }

        public async Task<Image> Upload(IFormFile file, Image image)
        {
            // 1 - Upload Image to Api (folder+file name+extension)
            var localPath = Path.Combine(webHostEnvironment.WebRootPath, "Images", $"{image.Name}{image.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);

            await file.CopyToAsync(stream);

            // 2 - Update Database
            var httpRequest = httpContextAccessor.HttpContext?.Request;
            if (httpRequest == null)
            {
                throw new InvalidOperationException("HTTP context is not available.");
            }

            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{image.Name}{image.FileExtension}";

            image.Url = urlPath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }

    }
}
