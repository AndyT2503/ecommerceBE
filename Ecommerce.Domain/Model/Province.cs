using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Model
{
    public class Province
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string CountryCode { get; set; }
        public virtual Country Country { get; set; }
        internal class ProvinceEntityConfiguration : IEntityTypeConfiguration<Province>
        {
            public void Configure(EntityTypeBuilder<Province> builder)
            {
                builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Province)}", MainDbContext.LocationSchema);
                builder.HasKey(x => x.Code);
                builder.Property(x => x.Name).IsRequired();
                builder.HasIndex(v => v.Name);
            }
        }
    }
}
