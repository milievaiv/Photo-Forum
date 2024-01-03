using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Repositories;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;

public class PostService : IPostService
{
    private readonly IPostRepository postRepository;
    public PostService(IPostRepository postRepository)
	{
        this.postRepository = postRepository;
	}

    public Comment Comment(User user, int postId, Comment comment)
    {
        return postRepository.Comment(user, postId, comment);
    }

    public Post Create(User user, Post post)
    {
        return postRepository.Create(user,post);
    }

    public bool Delete(int id)
    {
        return postRepository.Delete(id);
    }

    public Post EditPost(User user, int postId, Post editedPost)
    {
        return postRepository.EditPost(user, postId, editedPost);
    }

    public IList<Post> GetAll()
    {
        return postRepository.GetAll();
    }

    public Post GetById(int id)
    {
        return postRepository.GetById(id);
    }

    public IList<Post> GetUsersPost(User user)
    {
        return postRepository.GetUsersPost(user);
    }

    public Post Like(User user, int postId)
    {
        return postRepository.Like(user, postId);
    }
    public IList<Post> GetTopPosts()
    {
        return postRepository.GetTopPosts();
    }

    public IList<Post> RecentlyCreated()
    {
        return postRepository.RecentlyCreated();
    }
}
