using PhotoForum.Models;

namespace PhotoForum.Services.Contracts
{
    public interface ITokenService
    {
        string CreateToken(BaseUser user, string role);
    }
}
