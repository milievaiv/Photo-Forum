using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Models.QueryParameters;

namespace PhotoForum.Services.Contracts
{
    public interface IAdminsService
    {
        Admin Register(RegisterAdminModel registerAdminModel);
        bool Delete(int id);
        bool Block(string username);
        bool Unblock(string username);
        Admin GetAdminByUsername(string username);
        Log AddLog(string message);
        IList<Log> GetLastAddedLogs();
        IList<Admin> FilterBy(AdminQueryParameters filterParameters);


    }
}
