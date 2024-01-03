using PhotoForum.Models;
using PhotoForum.Models.Contracts;

namespace PhotoForum.Repositories.Contracts
{
    public interface IUsersRepository
    {
        Admin CreateAdmin(Admin user);
        User CreateUser(User user);
        User GetById(int id);
        Admin GetAdminByUsername(string username);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByFirstName(string firstName);
        IList<User> GetUsersL();
        User Update(int id, User user);
        bool Delete(int id);
        IList<User> FilterBy(UserQueryParameters searchParameters);
        bool UserExists(string username);
        bool Block(string username);
        bool Unblock(string username);
    }
}
