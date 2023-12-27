using PhotoForum.Models;

namespace PhotoForum.Services.Contracts
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
