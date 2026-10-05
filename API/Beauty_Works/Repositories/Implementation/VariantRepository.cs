using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Repositories.Implementation
{
    public class VariantRepository: IVariantRepository
    {
        private readonly ApplicationDbContext dbContext;

        public VariantRepository(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<Variant> CreateAsync(Variant variant)
        {
            await dbContext.Variants.AddAsync(variant);
            await dbContext.SaveChangesAsync();

            return variant;
        }

        public async Task<Variant?> DeleteAsync(int variantID)
        {
            var variant = await dbContext.Variants.Include(p => p.Products).Include(s => s.Subcategory).FirstOrDefaultAsync(v => v.ID == variantID);

            if (variant != null) 
            {
                dbContext.Variants.Remove(variant);
                await dbContext.SaveChangesAsync();
                return variant;
            }
            else 
            {
                return null;
            }
        }

        public async Task<IEnumerable<Variant>> GetAllAsync(string? sortBy, string? sortDirection, int? pageNumber = 1, int? pageSize = 100)
        {
            var variants = dbContext.Variants.Include(s => s.Subcategory).AsQueryable();

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        variants = isAsc ? variants.OrderBy(p => p.Name) : variants.OrderByDescending(p => p.Name);
                        break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            variants = variants.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await variants.ToListAsync();
        }

        public async Task<Variant?> GetByID(int variantID)
        {
            return await dbContext.Variants.Include(p => p.Products).Include(s => s.Subcategory)
                .FirstOrDefaultAsync(v => v.ID == variantID);
        }

        public async Task<Variant?> GetByNameAsync(string name)
        {
            return await dbContext.Variants.FirstOrDefaultAsync(v => v.Name == name);
        }

        public async Task<Variant?> UpdateAsync(Variant variant)
        {
            var existingVariant = await dbContext.Variants.Include(p => p.Products).Include(s => s.Subcategory)
                .FirstOrDefaultAsync(v => v.ID == variant.ID);

            if(existingVariant != null)
            {
                dbContext.Entry(existingVariant).CurrentValues.SetValues(variant);
                await dbContext.SaveChangesAsync();
                return existingVariant;
            }
            else
            {
                return null;
            }
        }
    }
}
