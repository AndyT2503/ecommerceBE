using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Ecommerce.Domain.Model.Common;

namespace Ecommerce.Domain.Model
{
    public class SaleCode
    {
        public string Code { get; set; }
        public int Percent { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    internal class SaleCodeEntityConfiguration : IEntityTypeConfiguration<SaleCode>
    {
        public void Configure(EntityTypeBuilder<SaleCode> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(SaleCode)}", MainDbContext.OrderSchema);
            builder.HasKey(x => x.Code);
            builder.HasIndex(x => x.Code);
            builder.HasIndex(x => x.Percent);
            builder.HasIndex(x => x.MaxPrice);
            builder.HasIndex(x => x.ValidUntil);
        }
    }
}
