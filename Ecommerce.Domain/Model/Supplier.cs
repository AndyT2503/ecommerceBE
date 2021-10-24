using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Ecommerce.Domain.Model.Common;

namespace Ecommerce.Domain.Model
{
    public class Supplier : BaseModel
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Code { get; set; }
        public virtual ICollection<SupplierProductType> SupplierProductTypes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    internal class SupplierEntityConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Supplier)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Code);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Logo).IsRequired();
        }
    }
}
