using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.QueryParameters;

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
        bool UpdateUserProfile(UserProfile model);
        IList<User> GetUsers();
        IList<User> FilterBy(UserQueryParameters searchParameters);
        IList<BaseUser> SearchBy(string filter);
        bool UserExists(string username);
        User GetUserByUsernameWithPosts(string username);

    }
}
