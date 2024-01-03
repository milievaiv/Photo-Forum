using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetUsersL();
        IList<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        Admin GetAdminByUsername(string username);
        User GetUserByUsername(string username);
        User GetUserByFirstName(string firstName);
        User GetUserByEmail(string email);
        //User Create(User user);
        User Update(int id, User user);
        bool Delete(int id);
        User Register(RegisterModel registerModel);
        bool Block(string username);
        bool Unblock(string username);
        //User AuthenticateUser(LoginModel loginModel);
    }
}
