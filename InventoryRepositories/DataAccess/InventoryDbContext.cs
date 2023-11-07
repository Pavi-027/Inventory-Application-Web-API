using InventoryEntities.IdentityEntities;
using InventoryEntities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryRepositories.DataAccess
{
    public class InventoryDbContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set;}

        protected override void OnModelCreating(ModelBuilder builder) //used to override the default behaviour
        {
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.FullName)
            .HasMaxLength(250);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.StreetAddress)
            .HasMaxLength(250);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.City)
            .HasMaxLength(250);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.State)
            .HasMaxLength(250);

            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .Property(e => e.Pincode)
            .HasMaxLength(250);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UpdatedByUser)
                .WithMany()
                .HasForeignKey(u => u.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
