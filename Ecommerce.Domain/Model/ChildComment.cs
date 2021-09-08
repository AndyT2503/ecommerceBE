using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecommerce.Domain.Model
{
    public class ChildComment : BaseModel
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }
    }

    internal class ChildCommentEntityConfiguration : IEntityTypeConfiguration<ChildComment>
    {
        public void Configure(EntityTypeBuilder<ChildComment> builder)
        {
            builder.ToTable($"{MainDbContext.DbTablePrefix}{nameof(ChildComment)}", MainDbContext.ProductSchema);
            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.Content);
            builder.HasIndex(x => x.CommentId);
            builder.HasOne(d => d.Comment).WithMany(d => d.ChildComments).HasForeignKey(d => d.CommentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
