using System;
using Shop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop.API.Database
{
    public class ShopContext: DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                .HasMany(c => c.Products);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders);

            modelBuilder.Seed();
        }
    }
}
