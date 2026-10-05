using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface ISubcategoryRepository
    {
        Task<Subcategory> CreateAsync(Subcategory subcategory);
        Task<IEnumerable<Subcategory>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100);
        Task<Subcategory?> GetByID(int subcategoryID);
        Task<bool?> GetHasVariantAsync(int subcategoryID);
        Task<Subcategory?> UpdateAsync(Subcategory subcategory);
        Task<Subcategory?> DeleteAsync(int subcategoryID);
    }
}
