using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class PostResponseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public Dictionary<string, List<string>> Comments { get; set; } = new Dictionary<string, List<string>>();
        public int Likes { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
