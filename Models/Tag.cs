using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string Name {get; set;}
        public List<Post> Posts { get; set;} = new List<Post>();
    }
}
