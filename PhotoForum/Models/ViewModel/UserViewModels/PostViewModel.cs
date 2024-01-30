namespace PhotoForum.Models.ViewModel.UserViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; } 
        public User User { get; set; }
        public bool IsLiked { get; set; }
    }
}
