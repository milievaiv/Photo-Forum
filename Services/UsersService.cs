using PhotoForum.Exceptions;
using PhotoForum.Models;
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

        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public User RegisterUser(RegisterModel registerModel)
        {
            var existingUser = usersRepository.UserExists(registerModel.Username);

            if (existingUser)
            {
                throw new DuplicateEntityException("User already exists!");
            }

            CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = registerModel.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                IsBlocked = false,
                IsDeleted = false
            };

            usersRepository.Create(user);

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public IList<User> GetAll()
        {
           return this.usersRepository.GetAll();
        }
        public User GetById(int id)
        {
            return this.usersRepository.GetById(id);
        }
        public User GetUserByUsername(string username)
        {
            return this.usersRepository.GetUserByUsername(username);
        }

        public IList<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this.usersRepository.FilterBy(filterParameters);
        }       

        public User Create(User user)
        {
            return this.usersRepository.Create(user);
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
