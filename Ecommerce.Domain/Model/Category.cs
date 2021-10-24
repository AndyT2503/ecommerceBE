using Ecommerce.Domain.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    internal class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Category)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Image);
            builder.HasIndex(x => x.Price);
            builder.HasIndex(x => x.ProductId);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.HasOne(d => d.Product).WithMany(d => d.Categories).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
