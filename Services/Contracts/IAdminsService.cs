using PhotoForum.Models.DTOs;
using PhotoForum.Models;

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


    }
}
