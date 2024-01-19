namespace PhotoForum.Models.ViewModel
{
    public class UserViewModel
    {
        public SearchUser SearchModel { get; set; }
        public List<User> Users { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
