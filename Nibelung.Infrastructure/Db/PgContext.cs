using Microsoft.EntityFrameworkCore;
using Nibelung.Domain.Models;
using System.Reflection;

namespace Nibelung.Infrastructure.Db
{
    public class PgContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAnswer> CommentAnswers { get; set; }
        public DbSet<NibelungFile> Files { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostFiles> PostFiles { get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        public PgContext()
        {
        }
        public PgContext(DbContextOptions<PgContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Nibelung.Domain"));
        }
    }
}