using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Ecommerce.Domain.Model
{
    public class Order : BaseModel
    {
        public string OrderCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string SaleCode { get; set; }
        public virtual SaleCode Sale { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    internal class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Order)}", MainDbContext.OrderSchema);
            builder.HasIndex(x => x.DistrictCode);
            builder.HasIndex(x => x.ProvinceCode);
            builder.HasIndex(x => x.OrderCode);
            builder.HasIndex(x => x.PaymentMethod);
            builder.HasIndex(x => x.PaymentStatus);
            builder.HasIndex(x => x.PaymentMethod);
            builder.HasIndex(x => x.Address);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CreatedAt);
            builder.Property(x => x.Status).IsRequired().HasDefaultValueSql("'Waiting'::text");
            builder.HasOne(x => x.Sale).WithMany().HasForeignKey(x => x.SaleCode).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
