using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Ecommerce.Domain.Model
{
    public class Comment : BaseModel
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ChildComment> ChildComments { get; set; }
    }

    internal class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(Comment)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.Content);
            builder.HasIndex(x => x.ProductId);
            builder.HasOne(d => d.Product).WithMany(d => d.Comments).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
