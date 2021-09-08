using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class SupplierProductType
    {
        public Guid SupplierId { get; set; }
        public Guid ProductTypeId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ProductType ProductType { get; set; }
    }

    internal class SupplierProductTypeEntityConfiguration : IEntityTypeConfiguration<SupplierProductType>
    {
        public void Configure(EntityTypeBuilder<SupplierProductType> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(SupplierProductType)}", MainDbContext.ProductSchema);
            builder.HasKey(x => new { x.SupplierId, x.ProductTypeId });
            builder.HasIndex(x => x.SupplierId);
            builder.HasIndex(x => x.ProductTypeId);
            builder.Property(x => x.SupplierId).IsRequired();
            builder.Property(x => x.ProductTypeId).IsRequired();
            builder.HasOne(x => x.Supplier).WithMany(x => x.SupplierProductTypes).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ProductType).WithMany(x => x.SupplierProductTypes).HasForeignKey(x => x.ProductTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
