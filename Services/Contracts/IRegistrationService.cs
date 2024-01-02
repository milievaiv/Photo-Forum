using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IRegistrationService
    {
        PasswordDTO Register(RegisterModel registerModel);
    }
}
