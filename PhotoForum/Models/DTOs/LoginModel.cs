using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
