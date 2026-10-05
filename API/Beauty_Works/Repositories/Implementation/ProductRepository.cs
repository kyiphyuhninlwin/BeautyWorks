using Beauty_Works.Data;
using Beauty_Works.Models.Domain;
using Beauty_Works.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Beauty_Works.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        // create and assign field
        private readonly ApplicationDbContext dbContext;

        //ctor to connect with dbcontext
        public ProductRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product?> AssignImageToProductAsync(int productID, int imageID)
        {
            var product = await dbContext.Products.FindAsync(productID);
            if (product != null)
            {
                product.ImageID = imageID;
                await dbContext.SaveChangesAsync();
            }

            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int productID)
        {
            var product = await dbContext.Products.Include(sc => sc.Subcategory).Include(s=> s.Status)
                .Include(b=> b.Brand).Include(v => v.Variants!).ThenInclude(v => v.Subcategory).Include(i => i.Image).FirstOrDefaultAsync(p => p.ID == productID);

            if(product != null)
            {
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
                return product;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int? subcategoryID, int? brandID, int? statusID, int? productTypeID, 
            string? sortBy, string? sortDirection, string? query = null, int? pageNumber = 1, int? pageSize = 100)
        {
            // query
            var products = dbContext.Products.Include(p => p.Subcategory).Include(p => p.Brand).Include(p => p.Status)
                .Include(p => p.Variants!).ThenInclude(v => v.Subcategory).Include(p => p.Image).AsQueryable();

            if (subcategoryID.HasValue)
            {
                products = products.Where(p => p.SubcategoryID == subcategoryID.Value);
            }

            if (brandID.HasValue)
            {
                products = products.Where(p => p.BrandID == brandID.Value);
            }

            if (statusID.HasValue)
            {
                products = products.Where(p => p.StatusID == statusID.Value);
            }

            if(productTypeID.HasValue)
            {
                products = products.Where(p => p.Subcategory != null && p.Subcategory.ProductTypeID == productTypeID.Value);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                products = products.Where(p =>
                                        (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(query)) ||
                                        (p.Brand != null && !string.IsNullOrEmpty(p.Brand.Name) && p.Brand.Name.Contains(query)));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

                switch (sortBy.ToLower())
                {
                    case "name":
                        products = isAsc ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                        break;
                    case "price":
                        products = isAsc ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                        break;
                    case "brand":
                        products = isAsc ? products.OrderBy(p => p.Brand!.Name) : products.OrderByDescending(p => p.Brand!.Name);
                        break;
                }
            }

            // Pagination
            // Page Number 1, Page size 5 => skip 0, take 5
            // Page Number 2, Page size 5 => skip 5, take 5 [6,7,8,9,10]
            // Page Number 3, Page size 5 => skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            products = products.Skip(skipResults ?? 0).Take(pageSize ?? 100);
            
            return await products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int productID)
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants!).ThenInclude(v => v.Subcategory).Include(i => i.Image).FirstOrDefaultAsync(p => p.ID == productID);
        }

        public async Task<Product?> GetByOrderIdAsync(int orderID)
        {
            return await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants!).ThenInclude(v => v.Subcategory).Include(i => i.Image).FirstOrDefaultAsync(p => p.OrderID == orderID);
        }

        public async Task<int> GetProductsTotal()
        {
            return await dbContext.Products.CountAsync();
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await dbContext.Products.Include(sc => sc.Subcategory).Include(s => s.Status)
                .Include(b => b.Brand).Include(v => v.Variants!).ThenInclude(v => v.Subcategory).Include(i => i.Image).FirstOrDefaultAsync(p => p.ID == product.ID);

            if (existingProduct != null)
            {
                dbContext.Entry(existingProduct).CurrentValues.SetValues(product);

                // Manually update the many-to-many Variants relationship
                existingProduct.Variants!.Clear();
                foreach (var variant in product.Variants!)
                {
                    existingProduct.Variants.Add(variant);
                }

                await dbContext.SaveChangesAsync();
                return existingProduct;
            }
            else
            {
                return null;
            }
        }
    }
}
