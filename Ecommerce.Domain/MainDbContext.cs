using Ecommerce.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class MainDbContext : DbContext
    {
        public const string DbTablePrefix = "";
        public const string ProductSchema = "product";
        public const string OrderSchema = "order";
        public const string AuthSchema = "auth";
        public const string NotifySchema = "notification";
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChildComment> ChildComments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SupplierProductType> SupplierProductTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<SaleCode> SaleCodes { get; set; }
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("pgcrypto")
                .HasPostgresExtension("uuid-ossp");

            builder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddAuditInfo();
            UpdateSoftDeleteStatuses();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity.GetType().GetProperty("IsDeleted") is not null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                            break;
                    }
                }
            }
        }

        private void AddAuditInfo()
        {
            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((BaseModel)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                ((BaseModel)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
