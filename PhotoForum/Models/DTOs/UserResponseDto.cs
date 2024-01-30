using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
