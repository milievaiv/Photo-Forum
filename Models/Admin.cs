using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Admin : BaseUser
    {
        public int Id { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
