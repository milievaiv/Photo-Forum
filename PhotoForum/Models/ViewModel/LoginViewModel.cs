using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
