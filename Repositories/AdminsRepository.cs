using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;

namespace PhotoForum.Repositories
{
    public class AdminsRepository : IAdminsRepository
    {
        private readonly PhotoForumContext context;

        public AdminsRepository(PhotoForumContext context)
        {
            this.context = context;
        }

        public Admin CreateAdmin(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();

            return admin;
        }
        public Admin GetAdminByUsername(string username)
        {
            var admin = GetAdmins().FirstOrDefault(u => u.Username == username);
            //if (admin == null) throw new EntityNotFoundException($"Admin with username {username} could not be found.");

            return admin;
        }

        private IQueryable<Admin> IQ_GetAdmins()
        {
            return context.Admins;
        }
        public IList<Admin> GetAdmins()
        {
            return IQ_GetAdmins().ToList();
        }

    }
}
