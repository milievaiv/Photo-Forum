using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public abstract class BaseUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
