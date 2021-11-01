using Ecommerce.Domain.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class OrderLog : BaseModel
    {
        public string Status { get; set; } 
        public DateTime Timestamp { get; set; }
        public virtual Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
    }

    internal class OrderLogEntityConfiguration : IEntityTypeConfiguration<OrderLog>
    {
        public void Configure(EntityTypeBuilder<OrderLog> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(OrderLog)}", MainDbContext.OrderSchema);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.OrderId);
            builder.HasOne(x => x.Order).WithMany(x => x.OrderLogs).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
