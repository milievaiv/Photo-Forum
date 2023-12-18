using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(32, MinimumLength =4)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
