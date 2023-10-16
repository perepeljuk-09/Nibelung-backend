using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class PostFiles
    {
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public Guid FileId { get; set; }
        public NibelungFile? File { get; set; }
    }
    public class ConfigurationPostFiles : IEntityTypeConfiguration<PostFiles>
    {
        public void Configure(EntityTypeBuilder<PostFiles> builder)
        {
            builder.ToTable("post_files","main");
            builder.HasKey(x => new { x.PostId, x.FileId });
            builder.Property(x => x.PostId).HasColumnName("post_id");
            builder.Property(x => x.FileId).HasColumnName("file_id");

            builder.HasOne(x => x.Post).WithMany(x => x.PostFiles).HasForeignKey(x => x.PostId);
            builder.HasOne(x => x.File).WithMany(x => x.PostFiles).HasForeignKey(x => x.FileId);
        }
    }
}
