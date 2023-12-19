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
            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public bool Delete(int id)
        {
            var userToDelete = context.Users.FirstOrDefault(user => user.Id == id)
                ?? throw new EntityNotFoundException($"User with id {id} not found.");
            context.Users.Remove(userToDelete);

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
            return context.Users;
        }
    }
}
