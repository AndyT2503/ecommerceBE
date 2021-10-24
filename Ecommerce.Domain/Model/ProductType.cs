using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Ecommerce.Domain.Model.Common;

namespace Ecommerce.Domain.Model
{
    public class ProductType : BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SupplierProductType> SupplierProductTypes { get; set; }
    }
    internal class ProductTypeEntityConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(ProductType)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Code);
        }
    }
}
