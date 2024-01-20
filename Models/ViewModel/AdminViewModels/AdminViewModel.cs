using PhotoForum.Models.Search;

namespace PhotoForum.Models.ViewModel.AdminViewModels
{
    public class AdminViewModel
    {
        public SearchUser SearchModel { get; set; }
        public List<Admin> Admins { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
