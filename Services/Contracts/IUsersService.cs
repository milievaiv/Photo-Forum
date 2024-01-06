using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetUsers();
        IQueryable<User> FilterBy(UserQueryParameters filterParameters);
        IList<User> SortBy(IQueryable<User> users, string sortBy);
        User Register(RegisterModel registerModel);
        User GetById(int id);
        User GetUserByUsername(string username);
        User GetUserByFirstName(string firstName);
        User GetUserByEmail(string email);
        User Update(int id, User user);
    }
}
