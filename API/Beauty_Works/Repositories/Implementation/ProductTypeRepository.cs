using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class ProductTypeRepository: IProductTypeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductTypeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductType> CreateAsync(ProductType productType)
        {
            await dbContext.ProductTypes.AddAsync(productType);
            await dbContext.SaveChangesAsync();

            return productType;
        }

        public async Task<ProductType?> DeleteAsync(int productTypeID)
        {
            var existingProductType = await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productTypeID);

            if (existingProductType != null)
            {
                dbContext.ProductTypes.Remove(existingProductType);
                await dbContext.SaveChangesAsync();
                return existingProductType;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100)
        {
            var productTypes = dbContext.ProductTypes.AsQueryable();

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        productTypes = isAsc ? productTypes.OrderBy(p => p.Name) : productTypes.OrderByDescending(p => p.Name);
                        break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            productTypes = productTypes.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await productTypes.ToListAsync();
        }

        public async Task<ProductType?> GetByID(int productTypeID)
        {
            return await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productTypeID);
        }

        public async Task<ProductType?> UpdateAsync(ProductType productType)
        {
            var existingProductType = await dbContext.ProductTypes.Include(pt => pt.Category).FirstOrDefaultAsync(p => p.ID == productType.ID);

            if(existingProductType != null)
            {
                dbContext.Entry(existingProductType).CurrentValues.SetValues(productType);
                await dbContext.SaveChangesAsync();
                return existingProductType;
            }
            else
            {
                return null;
            }
        }
    }
}
