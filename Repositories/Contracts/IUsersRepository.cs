using PhotoForum.Models;
using PhotoForum.Models.Contracts;

namespace PhotoForum.Repositories.Contracts
{
    public interface IUsersRepository
    {
        User CreateUser(User user);
        bool Delete(int id);
        bool Block(string username);
        bool Unblock(string username);
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByFirstName(string firstName);
        User GetUserByLastName(string lastName);
        User Update(int id, User user);
        IList<User> GetUsers();
        IList<User> FilterBy(UserQueryParameters searchParameters);
        bool UserExists(string username);

    }
}
