using PhotoForum.Exceptions;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories;
using System.Security.Cryptography;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Services
{
    public class VerificationService : IVerificationService
    {
        public readonly IUsersRepository usersRepository;
        public readonly IAdminsRepository adminsRepository;

        public VerificationService(IUsersRepository usersRepository, IAdminsRepository adminsRepository)
        {
            this.usersRepository = usersRepository;
            this.adminsRepository = adminsRepository;
        }
        public BaseUser AuthenticateUser(LoginModel loginModel)
        {
            var admin = adminsRepository.GetAdminByUsername(loginModel.Username);
            if (admin != null) return admin;
            var user = usersRepository.GetUserByUsername(loginModel.Username);
            if (user == null)
            {
                throw new UnauthorizedOperationException("Invalid username!");
            }
            if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedOperationException("Invalid password!");
            }

            return user; // Authentication successful
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}
