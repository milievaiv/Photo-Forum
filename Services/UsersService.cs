using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository repository;

        public UsersService(IUsersRepository repository) 
        {
            this.repository = repository;
        }
    }
}
