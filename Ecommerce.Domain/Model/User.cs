using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ecommerce.Domain.Model.Common;


namespace Ecommerce.Domain.Model
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
    }

    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(User)}", MainDbContext.AuthSchema);
            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.Password);
            builder.HasIndex(x => x.LastName);
            builder.HasIndex(x => x.FirstName);
            builder.HasIndex(x => x.Role);
        }
    }
}
