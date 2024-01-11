using PhotoForum.Models;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Services.Contracts
{
    public interface IPostService
    {
        Post Create(User user, Post post, List<Tag> tags);
        IList<Post> GetAll();
        Post GetById(int id);
        IList<Post> FilterBy(PostQueryParameters filterParameters);
        bool Delete(int id);
        IList<Post> GetUsersPost(User user);
        Post EditPost(User user, int postId, Post editedPost);
        Comment Comment(User user, int postId, Comment comment);
        Post Like(User user, int postId);
        Post Disike(User user, int postId);
        IList<Post> GetTopPosts();
        IList<Post> RecentlyCreated();
    }
}
