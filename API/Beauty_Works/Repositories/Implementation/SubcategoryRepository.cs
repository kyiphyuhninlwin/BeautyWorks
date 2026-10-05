using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SubcategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Subcategory> CreateAsync(Subcategory subcategory)
        {
            await dbContext.Subcategories.AddAsync(subcategory);
            await dbContext.SaveChangesAsync();

            return subcategory;
        }

        public async Task<Subcategory?> DeleteAsync(int subcategoryID)
        {
            var existingSubcategory = await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategoryID);

            if(existingSubcategory != null)
            {
                dbContext.Subcategories.Remove(existingSubcategory);
                await dbContext.SaveChangesAsync();
                return existingSubcategory;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100)
        {
            var subcategories = dbContext.Subcategories.AsQueryable();

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        subcategories = isAsc ? subcategories.OrderBy(p => p.Name) : subcategories.OrderByDescending(p => p.Name);
                        break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            subcategories = subcategories.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await subcategories.ToListAsync();
        }

        public async Task<Subcategory?> GetByID(int subcategoryID)
        {
            return await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategoryID);
        }

        public async Task<bool?> GetHasVariantAsync(int subcategoryID)
        {
            return await dbContext.Subcategories.Where(s => s.ID == subcategoryID)
                .Select(s => s.HasVariant)
                .FirstOrDefaultAsync();
        }

        public async Task<Subcategory?> UpdateAsync(Subcategory subcategory)
        {
            var existingSubcategory = await dbContext.Subcategories.Include(s => s.ProductType).FirstOrDefaultAsync(s => s.ID == subcategory.ID);

            if(existingSubcategory == null)
            {
               return null;
            }
            else
            {
                dbContext.Entry(existingSubcategory).CurrentValues.SetValues(subcategory);
                await dbContext.SaveChangesAsync();
                return existingSubcategory;
            }}
        
    }
}
