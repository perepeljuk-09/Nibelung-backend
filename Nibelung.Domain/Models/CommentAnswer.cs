using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class CommentAnswer
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Content { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ConfigurationCommentAnswer : IEntityTypeConfiguration<CommentAnswer>
    {
        public void Configure(EntityTypeBuilder<CommentAnswer> builder)
        {
            builder.ToTable("comment_answers", "main");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("answer_id");
            builder.Property(x => x.CommentId).HasColumnName("comment_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Content).HasColumnName("content");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        }
    }
}
