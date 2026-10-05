using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IBrandRepository
    {
        Task<Brand> CreateAsync(Brand brand);
        Task<IEnumerable<Brand>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100);
        Task<Brand?> GetByID(int brandID);
        Task<Brand?> UpdateAsync(Brand brand);
        Task<Brand?> DeleteAsync(int brandID);
    }
}
