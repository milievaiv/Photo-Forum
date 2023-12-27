using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoForum.Models
{
    public abstract class BaseUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

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
