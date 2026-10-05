using Beauty_Works.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Beauty_Works.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Image> Images { get; set; }
    }

}
