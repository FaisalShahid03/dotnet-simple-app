using Microsoft.EntityFrameworkCore;
using Day5MiniProject.Models;

namespace Day5MiniProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure SQL Server decimal precision for Price to avoid truncation warnings
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext ctx)
        {
            if (ctx.Categories.Any() || ctx.Products.Any()) return;

            var cat1 = new Category { Name = "Electronics" };
            var cat2 = new Category { Name = "Office" };

            ctx.Categories.AddRange(cat1, cat2);
            ctx.SaveChanges();

            var p1 = new Product { Name = "USB Cable", Description = "1m USB-A to USB-C", Price = 4.99m, CategoryId = cat1.Id };
            var p2 = new Product { Name = "Notebook", Description = "200 pages", Price = 2.49m, CategoryId = cat2.Id };
            var p3 = new Product { Name = "Monitor", Description = "24 inch", Price = 129.99m, CategoryId = cat1.Id };

            ctx.Products.AddRange(p1, p2, p3);
            ctx.SaveChanges();
        }
    }
}