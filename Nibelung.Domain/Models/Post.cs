using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int CountViews { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<PostLikes>? PostLikes { get; set; }
        public List<PostFiles>? PostFiles { get; set; }
        public List<Comment>? Comments { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ConfigurationPost : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("posts", "main");
            builder.HasKey(x => x.PostId);
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.CountViews).HasColumnName("count_views").HasDefaultValue(0);
            builder.Property(x => x.Title).HasColumnName("title");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
