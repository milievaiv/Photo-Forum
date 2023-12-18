using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        //public User User { get; set; }
        //public int PostId { get; set; }
    }
}
