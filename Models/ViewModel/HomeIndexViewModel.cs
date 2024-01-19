namespace PhotoForum.Models.ViewModel
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Post> RecentPhotos { get; set; }
        public IEnumerable<Post> TopPhotos { get; set; }
    }
}
