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
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder
        //    .UseSqlServer(@"Data Source=127.0.0.1;Initial Catalog=PhotoForum;User Id=sqlserver;Password=D?3F&>#(}HAmCOi%;")
        //    .EnableSensitiveDataLogging();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>().ToTable("BaseUsers");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");
        }
    }
}
