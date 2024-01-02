using PhotoForum.Exceptions;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using PhotoForum.Helpers;

namespace PhotoForum.Services
{
    public class AdminsService : IAdminsService
    {
        private readonly IRegistrationService registrationService;
        private readonly IUsersRepository usersRepository;
        public AdminsService(IUsersRepository usersRepository, IRegistrationService registrationService)
        {
            this.usersRepository = usersRepository;
            this.registrationService = registrationService;
        }

        public Admin Register(RegisterAdminModel registerAdminModel)
        {
            var dto = registrationService.Register(registerAdminModel);

            var admin = new Admin();
            admin.MapFrom(registerAdminModel, dto);

            usersRepository.CreateAdmin(admin);

            return admin;
        }
    }

}
