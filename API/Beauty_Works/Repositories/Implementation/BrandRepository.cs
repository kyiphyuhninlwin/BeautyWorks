using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Brand> CreateAsync(Brand brand)
        {
            await dbContext.Brands.AddAsync(brand);
            await dbContext.SaveChangesAsync();

            return brand;
        }

        public async Task<Brand?> DeleteAsync(int brandID)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brandID);

            if (existingBrand != null)
            {
                dbContext.Brands.Remove(existingBrand);
                await dbContext.SaveChangesAsync();
                return existingBrand;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100)
        {
            var brands = dbContext.Brands.AsQueryable();

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        brands = isAsc
                                ? brands.OrderBy(x => x.Name == null ? "" : x.Name.ToLower())
                                : brands.OrderByDescending(x => x.Name == null ? "" : x.Name.ToLower());

                        break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            brands = brands.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await brands.ToListAsync();
        }

        public async Task<Brand?> GetByID(int brandID)
        {
            return await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brandID);
        }

        public async Task<Brand?> UpdateAsync(Brand brand)
        {
            var existingBrand = await dbContext.Brands.FirstOrDefaultAsync(b => b.ID == brand.ID);

            if (existingBrand != null)
            {
                dbContext.Entry(existingBrand).CurrentValues.SetValues(brand);
                await dbContext.SaveChangesAsync();
                return existingBrand;
            }
            else
            {
                return null;
            }
        }
    }
}
