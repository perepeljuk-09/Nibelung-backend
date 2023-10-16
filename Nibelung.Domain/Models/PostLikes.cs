using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class PostLikes
    {
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime AddedAt { get; set; }
    }

    public class ConfigurationPostLikes : IEntityTypeConfiguration<PostLikes>
    {
        public void Configure(EntityTypeBuilder<PostLikes> builder)
        {
            builder.ToTable("post_likes","main");
            builder.HasKey(t => new { t.PostId, t.UserId });
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");

            builder.HasOne(x => x.Post).WithMany(x => x.PostLikes).HasForeignKey(x => x.PostId);
            builder.HasOne(x => x.User).WithMany(x => x.PostLikes).HasForeignKey(x => x.UserId);

        }
    }
}
