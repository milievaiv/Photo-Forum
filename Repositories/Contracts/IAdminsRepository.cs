using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.QueryParameters;

namespace PhotoForum.Repositories.Contracts
{
    public interface IAdminsRepository
    {
        Admin CreateAdmin(Admin admin);
        Admin GetAdminByUsername(string username);
        IList<Admin> GetAdmins();
        Log AddLog(string message);
        IList<Log> GetLastAddedLogs();
        IList<Admin> FilterBy(AdminQueryParameters filterParameters);

    }
}
