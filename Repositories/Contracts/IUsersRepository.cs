using PhotoForum.Models;

namespace PhotoForum.Repositories.Contracts
{
    public interface IUsersRepository
    {
        User Create(User user);
        User GetById(int id);
        BaseUser GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByFirstName(string firstName);
        IList<User> GetAll();
        User Update(int id, User user);
        bool Delete(int id);
        IList<User> FilterBy(UserQueryParameters searchParameters);
        bool UserExists(string username);
    }
}
