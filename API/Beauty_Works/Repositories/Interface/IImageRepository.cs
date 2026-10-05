using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> Upload(IFormFile file, Image image);

        Task<IEnumerable<Image>> GetAll();
    }
}
