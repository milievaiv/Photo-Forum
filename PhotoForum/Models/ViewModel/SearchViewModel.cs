namespace PhotoForum.Models.ViewModel
{
    public class SearchViewModel
    {
        public IEnumerable<BaseUser> Users { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public string SearchString { get; set; }

    }    
}
