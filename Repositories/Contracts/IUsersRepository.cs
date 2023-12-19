using PhotoForum.Models;

namespace PhotoForum.Repositories.Contracts
{
    public interface IUsersRepository
    {
        User Create(User user);
        User GetById(int id);
        IList<User> GetAll();
        User Update(int id, User user);
        bool Delete(int id);
    }
}
