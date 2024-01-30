using Microsoft.AspNetCore.Identity;
using PhotoForum.Exceptions;
using PhotoForum.Helpers;
using PhotoForum.Controllers.Data.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;
using PhotoForum.Repositories;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;
using System.Security.Cryptography;
using PhotoForum.Models.QueryParameters;

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

            EnsureUserUniqueEmail(user);
            usersRepository.CreateUser(user);

            return user;
        }      

        public IList<User> GetUsers()
        {
           return this.usersRepository.GetUsers();
        }
        public User GetUserById(int id)
        {
            return this.usersRepository.GetUserById(id);
        }
        public User GetUserByUsername(string username)
        {
            return this.usersRepository.GetUserByUsername(username);
        }                
        public User GetUserByFirstName(string firstName)
        {
            return this.usersRepository.GetUserByFirstName(firstName);
        }   
        public User GetUserByLastName(string lastName)
        {
            return this.usersRepository.GetUserByLastName(lastName);
        }           
        public User GetUserByEmail(string firstName)
        {
            return this.usersRepository.GetUserByEmail(firstName);
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.usersRepository.FilterBy(filterParameters);
        }
        public IList<BaseUser> SearchBy(string filter)
        {
            return this.usersRepository.SearchBy(filter);
        }
        public User Update(int id, User user)
        {
            EnsureUserUniqueEmail(user);
            var userToUpdate = usersRepository.Update(id,user);
            return userToUpdate;
        }
        public bool UpdateUserProfile(UserProfile model)
        {
            return usersRepository.UpdateUserProfile(model);
        }
        public User GetUserByUsernameWithPosts(string username)
        { 
            return usersRepository.GetUserByUsernameWithPosts(username);
        }

        private void EnsureUserUniqueEmail(User user)
        {
            var users = usersRepository.GetUsers();
            if (users.Any(u => u.Email == user.Email))
            {
                throw new DuplicateEmailException("This email is already taken.");
            }
        }     
    }
}
