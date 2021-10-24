using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using Ecommerce.Domain.Model.Common;

namespace Ecommerce.Domain.Model
{
    public class Comment : BaseModel
    {
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
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
            builder.HasIndex(x => x.IsAdmin);
            builder.HasIndex(x => x.UserId);
            builder.HasOne(d => d.Product).WithMany(d => d.Comments).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
