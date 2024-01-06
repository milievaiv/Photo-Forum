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
        IList<User> GetUsers();
        IList<BaseUser> GetBaseUsers();
        IList<Admin> GetAdmins();
        //IList<Post> GetPosts();
        User Update(int id, User user);
        bool Delete(int id);
        IQueryable<User> FilterBy(UserQueryParameters searchParameters);
        IList<User> SortBy(IQueryable<User> users, string sortBy);
        bool UserExists(string username);
        bool Block(string username);
        bool Unblock(string username);
    }
}
