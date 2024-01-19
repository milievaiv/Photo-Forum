namespace PhotoForum.Models.ViewModel
{
    public class PostViewModel
    {
        public SearchPost SearchModel { get; set; }
        public List<Post> Posts { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        // Sorting properties
        public string SortBy { get; set; } = "Id"; // Default sorting column
        public string SortOrder { get; set; } = "asc"; // Default sorting order
    }
}
