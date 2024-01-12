namespace PhotoForum.Models
{
    public class PostQueryParameters
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
    }
}
