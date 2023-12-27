using PhotoForum.Models;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetAll();
        IList<User> FilterBy();
        User GetById(int id);
        User Create(User user);
        User Update(int id, User user);
        bool Delete(int id);
        User RegisterUser(RegisterModel registerModel);
        User AuthenticateUser(LoginModel loginModel);
    }
}
