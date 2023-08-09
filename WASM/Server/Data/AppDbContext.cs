using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WASM.Shared.Models;

namespace WASM.Server.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            CreateRoles(builder);
            
            builder.Entity<Cart>()
                .HasOne(p => p.User)
                .WithMany().HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
               .HasOne<Cart>(p => p.Cart)
               .WithMany(p => p.CartItems)
               .HasForeignKey(p => p.CartId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                 .HasOne<Category>(p => p.Category)
                 .WithMany(p => p.Products)
                 .HasForeignKey(p => p.CategoryId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    
        protected void CreateRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "User", NormalizedName = "USER" }
                );
        }
    }
}
