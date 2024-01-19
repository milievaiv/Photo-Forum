using PhotoForum.Models;
using PhotoForum.Models.Contracts;

namespace PhotoForum.Repositories.Contracts
{
    public interface IAdminsRepository
    {
        Admin CreateAdmin(Admin admin);
        Admin GetAdminByUsername(string username);
        IList<Admin> GetAdmins();
        Log AddLog(string message);

    }
}
