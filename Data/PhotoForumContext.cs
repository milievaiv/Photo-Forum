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

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<User> users = new List<User>()
            {
                new User { Id = 1, Username = "admin", IsBlocked = false },
                new User { Id = 2, Username = "george", IsBlocked = false },
                new User { Id = 3, Username = "peter", IsBlocked = false }
            };
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
