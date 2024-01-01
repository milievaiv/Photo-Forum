using PhotoForum.Models;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetAll();
        IList<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User GetUserByUsername(string username);
        User Create(User user);
        User Update(int id, User user);
        bool Delete(int id);
        User RegisterUser(RegisterModel registerModel);
        //User AuthenticateUser(LoginModel loginModel);
    }
}
