using PhotoForum.Models.DTOs;
using PhotoForum.Models;

namespace PhotoForum.Services.Contracts
{
    public interface IAdminsService
    {
        Admin Register(RegisterAdminModel registerAdminModel);
    }
}
