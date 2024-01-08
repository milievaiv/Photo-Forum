namespace PhotoForum.Models.DTOs
{
    public class UserResponseAndPostDto : UserResponseDto
    {
        public int Id { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public IList<Post> Posts { get; set; }
    }
}
