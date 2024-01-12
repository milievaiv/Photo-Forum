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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>().ToTable("BaseUsers");
            modelBuilder.Entity<User>().ToTable("RegularUsers");
            modelBuilder.Entity<Admin>().ToTable("Admins");
        }
    }
}
