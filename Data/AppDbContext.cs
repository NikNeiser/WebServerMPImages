using Microsoft.EntityFrameworkCore;
using WebServerMPImages.Models;

namespace WebServerMPImages.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<ProductPhoto> ProductPhoto { get; set; }
    }
}
