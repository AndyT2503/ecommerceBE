using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Model
{
    public class District
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public string ProvinceCode { get; set; }
        public virtual Province Province { get; set; }
    }
    internal class DistrictEntityConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(District)}", MainDbContext.LocationSchema);
            builder.HasKey(x => x.Code);
            builder.Property(x => x.Name).IsRequired();
            builder.HasIndex(v => v.Name);

            builder.HasOne(x => x.Province).WithMany()
                .HasForeignKey(x => x.ProvinceCode);
        }
    }
}
