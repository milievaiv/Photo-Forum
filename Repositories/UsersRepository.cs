using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using PhotoForum.Helpers;

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
            var userToDelete = context.RegularUsers.FirstOrDefault(user => user.Id == id);
            ValidationHelper.Exists(userToDelete);

            userToDelete.IsDeleted = true;

            return context.SaveChanges() > 0;
        }

        public bool Block(string username)
        {
            var userToBlock = context.RegularUsers.FirstOrDefault(user => user.Username == username);
            ValidationHelper.Exists(userToBlock);

            if (userToBlock.IsBlocked == true) throw new InvalidOperationException("User is already blocked.");

            userToBlock.IsBlocked = true;

            return context.SaveChanges() > 0;
        }

        public bool Unblock(string username)
        {
            var userToUnblock = context.RegularUsers.FirstOrDefault(user => user.Username == username);
            ValidationHelper.Exists(userToUnblock);

            if (userToUnblock.IsBlocked == false) throw new InvalidOperationException("User is not blocked.");

            userToUnblock.IsBlocked = false;

            return context.SaveChanges() > 0;
        }

        public User GetById(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.Id == id);
            ValidationHelper.Exists(user);

            return user;
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            IQueryable<User> result = IQ_GetUsers();
            result = FilterByUsername(result, filterParameters.Username);
            result = FilterByEmail(result, filterParameters.Email);
            result = FilterByFirstName(result, filterParameters.FirstName);
            result = FilterByLastName(result, filterParameters.LastName);

            return result.ToList();
        }
        public BaseUser GetBaseUserByUsername(string username)
        {
            var baseuser = GetBaseUsers().FirstOrDefault(u => u.Username == username);
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
            ValidationHelper.Exists(user);

            return user;
        }
        public User GetUserByEmail(string email)
        {
            var user = GetUsers().FirstOrDefault(u => u.Email == email);
            ValidationHelper.Exists(user);

            return user;
        }
        public User GetUserByFirstName(string firstName)
        {
            var user = GetUsers().FirstOrDefault(u => u.FirstName == firstName);
            ValidationHelper.Exists(user);

            return user;
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

        private IQueryable<User> IQ_GetUsers()
        {
            return context.RegularUsers;                
               
        }
        public IList<User> GetUsers()
        {
            return IQ_GetUsers().ToList();
        }

        //private IQueryable<Post> IQ_GetPosts()
        //{
        //    return context.Posts;

        //}
        //public IList<Post> GetPosts()
        //{
        //    return IQ_GetPosts().ToList();
        //}

        private IQueryable<BaseUser> IQ_GetBaseUsers()
        {
            return context.BaseUsers;
        }
        public IList<BaseUser> GetBaseUsers()
        {
            return IQ_GetBaseUsers().ToList();
        }

        private IQueryable<Admin> IQ_GetAdmins()
        {
            return context.Admins;
        }
        public IList<Admin> GetAdmins()
        {
            return IQ_GetAdmins().ToList();
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

        private static IQueryable<User> FilterByLastName(IQueryable<User> users, string lastName)
        {
            if (!string.IsNullOrEmpty(lastName))
            {
                return users.Where(user => user.FirstName.Contains(lastName));
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
