using PhotoForum.Models;

namespace PhotoForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public User User {  get; set; }
        [Required]
        [StringLength(64, MinimumLength=16)]
        public string Title { get; set; }
        [Required]
        [StringLength(8192, MinimumLength = 32)]
        public string Content { get; set; }
        public IList<Comment> comment { get; set; }
        public int Likes { get; set; }
    }
}
