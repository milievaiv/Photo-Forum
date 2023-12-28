using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoForum.Models;

namespace PhotoForum.Data
{
    public class ApplicationDbContext : IdentityDbContext<BaseUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
