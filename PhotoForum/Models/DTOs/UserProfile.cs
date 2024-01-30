using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string LastName { get; set; }
    }
}
