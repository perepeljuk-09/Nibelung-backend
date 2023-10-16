using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public string? Content { get; set; }
        public List<CommentAnswer>? CommentAnswers { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ConfigurationComment : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments","main");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("comment_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.Content).HasColumnName("content");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(x => x.Post).WithMany(x => x.Comments).HasForeignKey(x => x.PostId);
            builder.HasMany(x => x.CommentAnswers).WithOne(x => x.Comment).HasForeignKey(x => x.CommentId);
        }
    }
}
