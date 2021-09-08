using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class Rating : BaseModel
    {
        public string Comment { get; set; }
        public int Rate { get; set; }
        public string ImageUrl { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    internal class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Rating)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Comment);
            builder.HasIndex(x => x.Rate);
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.ImageUrl);
            builder.HasOne(x => x.Product).WithMany(x => x.Ratings).HasForeignKey(x => x.ProductId);
        }
    }
}
