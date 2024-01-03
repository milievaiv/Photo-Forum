using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PhotoForum.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly PhotoForumContext context;

        public UsersRepository(PhotoForumContext context)
        {
            this.context = context;
        }

        public User CreateUser(User user)
        {
            context.RegularUsers.Add(user);
            context.SaveChanges();

            return user;
        }

        public Admin CreateAdmin(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();

            return admin;
        }

        public bool Delete(int id)
        {
            var userToDelete = context.RegularUsers.FirstOrDefault(user => user.Id == id)
                ?? throw new EntityNotFoundException($"User with id {id} not found.");
            context.RegularUsers.Remove(userToDelete);

            return context.SaveChanges() > 0;
        }

        public bool Block(string username)
        {
            var userToBlock = context.RegularUsers.FirstOrDefault(user => user.Username == username)
                ?? throw new EntityNotFoundException($"User with username {username} not found.");
            userToBlock.IsBlocked = true;

            return context.SaveChanges() > 0;
        }

        public bool Unblock(string username)
        {
            var userToUnblock = context.RegularUsers.FirstOrDefault(user => user.Username == username)
                ?? throw new EntityNotFoundException($"User with username {username} not found.");
            userToUnblock.IsBlocked = false;

            return context.SaveChanges() > 0;
        }

        public bool UpgradeToAdmin(string username, string phoneNumber)
        {
            var userToUpgrade = GetBaseUserByUsername(username);

            Delete(userToUpgrade.Id);

            var admin = new Admin
            {
                Id = userToUpgrade.Id,
                PhoneNumber = phoneNumber,
            };

            context.Admins.Add(admin);

            return context.SaveChanges() > 0;
        }

        public User GetById(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            IQueryable<User> result = GetUsers();
            result = FilterByUsername(result, filterParameters.Username);
            result = FilterByEmail(result, filterParameters.Email);
            result = FilterByFirstName(result, filterParameters.FirstName);

            return result.ToList();
        }
        public BaseUser GetBaseUserByUsername(string username)
        {
            var baseuser = GetAll().FirstOrDefault(u => u.Username == username);
            return baseuser;
        }
        public Admin GetAdminByUsername(string username)
        {
            var admin = GetAdmins().FirstOrDefault(u => u.Username == username);
            return admin;
        }
        public User GetUserByUsername(string username)
        {
            var user = GetUsers().FirstOrDefault(u => u.Username == username);

            return user ?? throw new EntityNotFoundException($"User with username: {username} doesn't exist.");
        }
        public User GetUserByEmail(string email)
        {
            var user = GetUsers().FirstOrDefault(u => u.Email == email);

            return user ?? throw new EntityNotFoundException($"User with email: {email} doesn't exist.");
        }
        public User GetUserByFirstName(string firstName)
        {
            var user = GetUsers().FirstOrDefault(u => u.FirstName == firstName);

            return user ?? throw new EntityNotFoundException($"User with first name: {firstName} doesn't exist.");
        }

        public User Update(int id, User user)
        {
            var userToUpdate = GetById(id);

            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;

            context.Update(userToUpdate);
            context.SaveChanges();

            return userToUpdate;
        }

        private IQueryable<User> GetUsers()
        {
            return context.RegularUsers
                .Include(x => x.Posts)
                .Include(x => x.Comments);                
               
        }
        public IList<User> GetUsersL()
        {
            return GetUsers().ToList();
        }
        private IQueryable<BaseUser> GetAll()
        {
            return context.BaseUsers;
        }
        private IQueryable<Admin> GetAdmins()
        {
            return context.Admins;
        }
        
        private static IQueryable<User> FilterByUsername(IQueryable<User> users, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return users.Where(user => user.Username.Contains(username));
            }
            else
            {
                return users;
            }
        }

        private static IQueryable<User> FilterByEmail(IQueryable<User> users, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return users.Where(user => user.Email.Contains(email));
            }
            else
            {
                return users;
            }
        }

        private static IQueryable<User> FilterByFirstName(IQueryable<User> users, string firstName)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                return users.Where(user => user.FirstName.Contains(firstName));
            }
            else
            {
                return users;
            }
        }
        public bool UserExists(string username)
        {
            return context.BaseUsers.Any(user => user.Username == username);
        }
    }
}
