using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Domain.Model
{
    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    internal class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Country)}", MainDbContext.LocationSchema);
            builder.HasKey(x => x.Code);

            builder.Property(x => x.Name).IsRequired();
            builder.HasIndex(x => x.Name);
        }
    }
}
