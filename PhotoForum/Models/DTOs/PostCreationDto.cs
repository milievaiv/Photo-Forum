using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class PostCreationDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Creator { get; set; }
        public int Likes { get; set; }

    }
}
