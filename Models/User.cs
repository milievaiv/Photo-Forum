using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class User : BaseUser
    {
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
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public IList<Post> Posts { get; set; }
        public IList<Comment> Comments { get; set; }
        
    }
}
