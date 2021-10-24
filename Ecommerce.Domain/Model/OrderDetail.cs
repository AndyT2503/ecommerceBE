using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Ecommerce.Domain.Model.Common;

namespace Ecommerce.Domain.Model
{
    public class OrderDetail : BaseModel
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    internal class OrderDetailEntityConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(OrderDetail)}", MainDbContext.OrderSchema);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.Quantity);
            builder.HasIndex(x => x.Price);
            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
