using Beauty_Works.Models.Domain;

namespace Beauty_Works.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int productID);
        Task<Product?> GetByOrderIdAsync(int orderID);
        Task<IEnumerable<Product>> GetAllAsync(int? subcategoryID, int? brandID, int? statusId, int? productTypeID,
            string? sortBy, string? sortDirection, string? query = null, int? pageNumber = 1, int? pageSize = 100);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(int productID);
        Task<int> GetProductsTotal();
        Task<Product?> AssignImageToProductAsync(int productID, int imageID);
    }
}
