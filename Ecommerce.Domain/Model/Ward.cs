using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Model
{
    public class Ward
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DistrictCode { get; set; }
        public virtual District District { get; set; }
    }

    internal class WardEntityConfiguration : IEntityTypeConfiguration<Ward>
    {
        public void Configure(EntityTypeBuilder<Ward> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Ward)}", MainDbContext.LocationSchema);
            builder.HasKey(x => x.Code);
            builder.Property(x => x.Name).IsRequired();
            builder.HasIndex(x => x.Name);

            builder.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictCode);
        }
    }
}
