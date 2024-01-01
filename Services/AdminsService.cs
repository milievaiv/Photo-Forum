using PhotoForum.Exceptions;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories;
using PhotoForum.Repositories.Contracts;

namespace PhotoForum.Services
{
    public class AdminsService
    {
        private readonly IUsersRepository usersRepository;
        public AdminsService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }
    }

}
