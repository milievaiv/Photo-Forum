using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;

namespace PhotoForum.Helpers
{
    public static class UserExtensions
    {
        public static void MapFrom(this BaseUser user, IUserRegistrationModel registerModel, PasswordDTO dto)
        {
            user.Username = registerModel.Username;
            user.FirstName = registerModel.FirstName;
            user.LastName = registerModel.LastName;
            user.Email = registerModel.Email;
            user.PasswordHash = dto.PasswordHash;
            user.PasswordSalt = dto.PasswordSalt;

            if (user is Admin admin && registerModel is RegisterAdminModel adminModel)
            {
                admin.PhoneNumber = adminModel.PhoneNumber;
            }

            // Add additional mapping logic for other user types if needed
        }
    }
}
