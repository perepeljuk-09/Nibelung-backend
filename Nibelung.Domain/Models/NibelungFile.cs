using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class NibelungFile
    {
        public Guid Id { get; set; }
        public string? Path { get; set; }
        public string? Name { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<PostFiles>? PostFiles { get; set; }
        public DateTime AddedAt { get; set; }
    }
    public class ConfigurationFile : IEntityTypeConfiguration<NibelungFile>
    {
        public void Configure(EntityTypeBuilder<NibelungFile> builder)
        {
            builder.ToTable("files", "main");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("file_id");
            builder.Property(x => x.Path).HasColumnName("path");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");

            builder.HasOne(x => x.User).WithMany(x => x.Files).HasForeignKey(x => x.UserId);

        }
    }
}
