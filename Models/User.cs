using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class User : BaseUser
    {
        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public IList<Post> Posts { get; set; } = new List<Post>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}
