using PhotoForum.Exceptions;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;
using System.Security.Cryptography;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUsersRepository usersRepository;
        public RegistrationService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public PasswordDTO Register(RegisterModel registerModel)
        {
            var existingUser = usersRepository.UserExists(registerModel.Username);

            if (existingUser)
            {
                throw new DuplicateEntityException("User already exists!");
            }

            CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var dto = new PasswordDTO
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            return dto;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
