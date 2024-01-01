using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Repositories.Contracts;

namespace PhotoForum.Services.Contracts
{
    public interface IVerificationService
    {
        BaseUser AuthenticateUser(LoginModel loginModel);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);

    }
}
