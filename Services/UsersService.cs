using Microsoft.AspNetCore.Identity;
using PhotoForum.Exceptions;
using PhotoForum.Helpers;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;
using PhotoForum.Repositories;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;
using System.Security.Cryptography;

namespace PhotoForum.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IRegistrationService registrationService;

        public UsersService(IUsersRepository usersRepository, IRegistrationService registrationService)
        {
            this.usersRepository = usersRepository;
            this.registrationService = registrationService;
        }
        public User Register(RegisterModel registerModel)
        {
            var dto = registrationService.Register(registerModel);

            var user = new User();
            user.MapFrom(registerModel, dto);

            usersRepository.CreateUser(user);

            return user;
        }
        public bool Block(string username)
        {
            return this.usersRepository.Block(username);
        }        
        public bool Unblock(string username)
        {
            return this.usersRepository.Unblock(username);
        }
        public IList<User> GetUsersL()
        {
           return this.usersRepository.GetUsersL();
        }
        public User GetById(int id)
        {
            return this.usersRepository.GetById(id);
        }
        public Admin GetAdminByUsername(string username)
        {
            return this.usersRepository.GetAdminByUsername(username);
        }

        public User GetUserByUsername(string username)
        {
            return this.usersRepository.GetUserByUsername(username);
        }        
        
        public User GetUserByFirstName(string firstName)
        {
            return this.usersRepository.GetUserByFirstName(firstName);
        }   
        
        public User GetUserByEmail(string firstName)
        {
            return this.usersRepository.GetUserByEmail(firstName);
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.usersRepository.FilterBy(filterParameters);
        }       

        public User Update(int id, User user)
        {
            EnsureUserUniqueEmail(user);
            var userToUpdate = usersRepository.Update(id,user);
            return userToUpdate;
        }

        public bool Delete(int id)
        {
            return usersRepository.Delete(id);
        }

        private void EnsureUserUniqueEmail(User user)
        {
            if (usersRepository.GetUserByEmail(user.Email) != null) 
            {
                throw new DuplicateEntityException("This email is already taken.");
            }
        }

        
    }
}
