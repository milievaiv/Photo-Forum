using PhotoForum.Models.Contracts;

namespace PhotoForum.Models.DTOs
{
    public class RegisterModel : IUserRegistrationModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Implement other properties for RegisterModel
    }
}
