using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Domain.Models
{
    public class RefreshToken
    {
        public int TokenId { get; set; }
        public string? Token { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
    public class ConfigurationRefreshToken : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens","main");
            builder.HasKey(x => x.TokenId);
            builder.Property(x => x.TokenId).HasColumnName("token_id");
            builder.Property(x => x.Token).HasColumnName("token");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");
            builder.Property(x => x.ExpiredAt).HasColumnName("expired_at");
        }
    }
}
