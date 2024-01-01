using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;

namespace PhotoForum.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly PhotoForumContext context;

        public UsersRepository(PhotoForumContext context)
        {
            this.context = context;
        }

        public User Create(User user)
        {
            context.RegularUsers.Add(user);
            context.SaveChanges();

            return user;
        }

        public bool Delete(int id)
        {
            var userToDelete = context.RegularUsers.FirstOrDefault(user => user.Id == id)
                ?? throw new EntityNotFoundException($"User with id {id} not found.");
            context.RegularUsers.Remove(userToDelete);

            return context.SaveChanges() > 0;
        }

        public IList<User> GetAll()
        {
            return GetUsers().ToList();
        }

        public User GetById(int id)
        {
            var user = GetUsers().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public IList<User> SearchBy(UserQueryParameters searchParameters)
        {
            IQueryable<User> result = GetUsers();
            result = SearchByUsername(result, searchParameters.Username);
            result = SearchByEmail(result, searchParameters.Email);
            result = SearchByFirstName(result, searchParameters.FirstName);

            return result.ToList();
        }
        public BaseUser GetUserByUsername(string username)
        {
            var admin = GetAdmins().FirstOrDefault(u => u.Username == username);

            if (admin != null) return admin;

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
            return context.RegularUsers;
        }

        private IQueryable<Admin> GetAdmins()
        {
            return context.Admins;
        }

        private static IQueryable<User> SearchByUsername(IQueryable<User> users, string username)
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

        private static IQueryable<User> SearchByEmail(IQueryable<User> users, string email)
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

        private static IQueryable<User> SearchByFirstName(IQueryable<User> users, string firstName)
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
            return context.RegularUsers.Any(user => user.Username == username);
        }
    }
}
