using Microsoft.EntityFrameworkCore;
using PhotoForum.Models;

namespace PhotoForum.Data
{
    public class PhotoForumContext : DbContext
    {
        public PhotoForumContext(DbContextOptions<PhotoForumContext> options)
            : base(options)
        {
        }

        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<User> RegularUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Like> Likes { get; set; }

        public override int SaveChanges()
        {
            var currentTime = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Log>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Timestamp").CurrentValue = currentTime;
                }
            }

            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure BaseUser entity
            modelBuilder.Entity<BaseUser>()
                .HasKey(b => b.Id);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasBaseType<BaseUser>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Creator)
                .OnDelete(DeleteBehavior.Cascade); // Assuming a user can have multiple posts

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade); // Assuming a user can have multiple comments

            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedPosts)
                .WithOne(l => l.User)
                .OnDelete(DeleteBehavior.Cascade); // Assuming a user can like multiple posts

            // Configure Admin entity
            modelBuilder.Entity<Admin>()
                .HasBaseType<BaseUser>()
                .ToTable("Admins");

            // Configure Post entity
            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Assuming a post can have multiple likes

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(j => j.ToTable("PostTag")); // Assuming a post can have multiple tags

            // Configure Comment entity
            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            // Configure Like entity
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.PostId });

            // Configure Tag entity
            modelBuilder.Entity<Tag>()
                .HasKey(t => t.Id);

            // Configure Log entity
            modelBuilder.Entity<Log>()
                .HasKey(l => l.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
