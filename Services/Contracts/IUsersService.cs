using PhotoForum.Models;

namespace PhotoForum.Services.Contracts
{
    public interface IUsersService
    {
        IList<User> GetAll();
        IList<User> FilterBy();
        User GetById(int id);
        User Create(User beer);
        User Update(int id, User user);
        bool Delete(int id);
    }
}
