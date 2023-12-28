using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoForum.Models
{
    public abstract class BaseUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string LastName { get; set; }
    }
}
