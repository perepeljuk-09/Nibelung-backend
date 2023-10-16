using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nibelung.Domain.Enums;

namespace Nibelung.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public DateOnly Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public RefreshToken? RefreshToken { get; set; }
        public List<Post>? Posts { get; set; }
        public List<NibelungFile>? Files { get; set; }
        public List<PostLikes>? PostLikes { get; set; }
        public List<CommentAnswer>? CommentAnswers { get; set; }
        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ConfigurationUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users","main");
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.FirstName).HasColumnName("first_name");
            builder.Property(x => x.Birthday).HasColumnName("birthday");
            builder.Property(x => x.Gender).HasColumnName("gender");
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.PasswordSalt).HasColumnName("password_salt");
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash");
            builder.Property(x => x.AddedAt).HasColumnName("added_at").HasDefaultValueSql("now()");
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

            builder.HasOne(x => x.RefreshToken).WithOne(x => x.User).HasForeignKey<RefreshToken>(x => x.UserId);
            builder.HasMany(x => x.Posts).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Files).WithOne(x => x.User).HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.CommentAnswers).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}