using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetUsers();
        IList<User> FilterBy(UserQueryParameters filterParameters);
        IList<BaseUser> SearchBy(string filter);
        User Register(RegisterModel registerModel);
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByFirstName(string firstName);
        User GetUserByLastName(string firstName);
        User GetUserByEmail(string email);
        User Update(int id, User user);
        User GetUserByUsernameWithPosts(string username);
    }
}
