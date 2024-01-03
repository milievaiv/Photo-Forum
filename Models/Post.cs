using PhotoForum.Models;
using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(64, MinimumLength=16)]
        public string Title { get; set; }
        [Required]
        [StringLength(8192, MinimumLength = 32)]
        public string Content { get; set; }
        public int UserId { get; set; }

        //Nav prop
        public User User { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public int Likes { get; set; }
        public DateTime Date { get; set; }
    }
}
