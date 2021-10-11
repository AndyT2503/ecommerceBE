using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class Notification : BaseModel
    {
        public string GroupName { get; set; }
        public Guid? UserId { get; set; }
        public bool Seen { get; set; }
        public object MetaData { get; set; }
        public string EventName { get; set; }
    }

    internal class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Notification)}", MainDbContext.NotifySchema);
            builder.HasIndex(x => x.GroupName);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.EventName);
            builder.HasIndex(x => x.CreatedAt);
            builder.Property(e => e.MetaData)
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'{ }'::jsonb");
        }
    }
}
