using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class LoginModel
    {
        public string Username { get; set; } 
        public string Password { get; set; }
    }
}
