using PhotoForum.Models;

namespace PhotoForum.Repositories.Contracts
{
    public interface IUsersRepository
    {
        User Create(User user);
        User GetById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByFirstName(string firstName);
        IList<User> GetAll();
        User Update(int id, User user);
        bool Delete(int id);
        IList<User> SearchBy(UserQueryParameters searchParameters);
        bool UserExists(string username);
    }
}
