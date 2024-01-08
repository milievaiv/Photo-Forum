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

        public bool Delete(int id)
        {
            User userToDelete = GetUserById(id);

            if (userToDelete.IsDeleted == true) throw new InvalidOperationException("User is already deleted.");

            userToDelete.IsDeleted = true;

            return context.SaveChanges() > 0;
        }

        public bool Block(string username)
        {
            User userToBlock = GetUserByUsername(username);

            if (userToBlock.IsBlocked == true) throw new InvalidOperationException("User is already blocked.");

            userToBlock.IsBlocked = true;

            return context.SaveChanges() > 0;
        }

        public bool Unblock(string username)
        {
            User userToUnblock = GetUserByUsername(username);

            if (userToUnblock.IsBlocked == false) throw new InvalidOperationException("User is not blocked.");

            userToUnblock.IsBlocked = false;

            return context.SaveChanges() > 0;
        }

        public BaseUser GetBaseUserByUsername(string username)
        {
            var baseuser = GetBaseUsers().FirstOrDefault(u => u.Username == username);
            //if (baseuser == null) throw new EntityNotFoundException($"Base user with username {username} could not be found.");
            
            return baseuser;
        }

        public User GetUserById(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.Id == id);
            //if (user == null) throw new EntityNotFoundException($"User with id {id} could not be found.");

            return user;
        }
        public User GetUserByUsername(string username)
        {
            var user = GetUsers().FirstOrDefault(u => u.Username == username);
            //if (user == null) throw new EntityNotFoundException($"User with username {username} could not be found.");
            
            return user;
        }
        public User GetUserByEmail(string email)
        {
            var user = GetUsers().FirstOrDefault(u => u.Email == email);
            //if (user == null) throw new EntityNotFoundException($"User with email {email} could not be found.");

            return user;
        }
        public User GetUserByFirstName(string firstName)
        {
            var user = GetUsers().FirstOrDefault(u => u.FirstName == firstName);
            //if (user == null) throw new EntityNotFoundException($"User with first name {firstName} could not be found.");

            return user;
        }
        public User GetUserByLastName(string lastName)
        {
            var user = GetUsers().FirstOrDefault(u => u.LastName == lastName);
            //if (user == null) throw new EntityNotFoundException($"User with last name {lastName} could not be found.");

            return user;
        }

        public User Update(int id, User user)
        {
            var userToUpdate = GetUserById(id);

            userToUpdate.Email = user.Email;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
           

            context.Update(userToUpdate);
            context.SaveChanges();

            return userToUpdate;
        }

        private IQueryable<User> IQ_GetUsers()
        {
            return context.RegularUsers
                .Include(x => x.Posts)
                .Include(x => x.Comments);
        }
        public IList<User> GetUsers()
        {
            return IQ_GetUsers().ToList();
        }

        private IQueryable<BaseUser> IQ_GetBaseUsers()
        {
            return context.BaseUsers;
        }
        public IList<BaseUser> GetBaseUsers()
        {
            return IQ_GetBaseUsers().ToList();
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            IQueryable<User> result = IQ_GetUsers();

            result = FilterByFirstName(result, filterParameters.FirstName);
            result = FilterByLastName(result, filterParameters.LastName);
            result = FilterByUsername(result, filterParameters.FirstName);
            result = FilterByEmail(result, filterParameters.LastName);
            result = SortBy(result, filterParameters.SortBy);
            return result.ToList();
        }
        private static IQueryable<User> FilterByUsername(IQueryable<User> users, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return users.Where(user => user.FirstName.Contains(username));
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
                return users.Where(user => user.FirstName.Contains(email));
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

        public static IQueryable<User> SortBy(IQueryable<User> users, string sortBy)
        {
            switch (sortBy)
            {
                case "username":
                    users = SortByUsername(users);
                    break;
                case "firstName":
                    users = SortByFirstName(users);
                    break;
                case "lastName":
                    users = SortByLastName(users);
                    break;
                case "email":
                    users = SortByEmail(users);
                    break;
            }

            return users;
        }
        private static IQueryable<User> SortByUsername(IQueryable<User> users)
        {
            return users.OrderBy(user => user.Username);
        }
        private static IQueryable<User> SortByEmail(IQueryable<User> users)
        {
            return users.OrderBy(user => user.Email);

        }
        private static IQueryable<User> SortByFirstName(IQueryable<User> users)
        {
            return users.OrderBy(user => user.FirstName);
        }
        private static IQueryable<User> SortByLastName(IQueryable<User> users)
        {
            return users.OrderBy(user => user.LastName);
        }

        public bool UserExists(string username)
        {
            return context.BaseUsers.Any(user => user.Username == username);
        }
    }
}
