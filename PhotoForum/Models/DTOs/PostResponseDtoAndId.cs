using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class PostResponseDtoAndId : PostResponseDto
    {
        public int Id { get; set; }
    }
}
