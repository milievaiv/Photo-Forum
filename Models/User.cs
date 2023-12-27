using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class User : BaseUser
    {
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
