using Microsoft.EntityFrameworkCore;
using RealTalk.Api.Entities;

namespace RealTalk.Api.Data
{
    public class RealTalkDbContext : DbContext
    {
        public RealTalkDbContext(DbContextOptions<RealTalkDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
