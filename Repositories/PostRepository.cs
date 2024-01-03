using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly PhotoForumContext context;

    public PostRepository(PhotoForumContext context)
    {
        this.context = context;
    }
    public IQueryable<Post> GetMostRecentRecords(int count)
    {
        return context.Posts
            .OrderByDescending(x => x.Id)
            .Take(count);
    }
    public Post Create(User user, Post post)
    {
        post.User = user;
        context.Posts.Add(post);
        //post.User.Posts.Add(post);
        //user.Posts.Add(post);
        context.SaveChanges();
        
        return post;
    }
    public IList<Post> GetAll()
    {
        return GetPosts().ToList();
    }
    public Post GetById(int id)
    {
        var post = GetPosts().FirstOrDefault(p => p.Id == id);

        return post ?? throw new EntityNotFoundException($"Post with id {id} not found.");
    }

    public bool Delete(int id)
    {
        var postToDelete = context.Posts.FirstOrDefault(p => p.Id == id)
                ?? throw new EntityNotFoundException($"Post with id {id} not found.");

        context.Posts.Remove(postToDelete);

        return context.SaveChanges() > 0;
    }
    public IList<Post> GetUsersPost(User user)
    {
        var userPosts = user.Posts.ToList();

        return userPosts;
    }
    public Post EditPost(User user, int postId, Post editedPost)
    {
        var postToEdit = FindPostFromUser(user, postId);

        postToEdit.Title = editedPost.Title;
        postToEdit.Content = editedPost.Content;

        context.Update(postToEdit);
        context.SaveChanges();

        return postToEdit;
    }

    public Comment Comment(User user, int postId, Comment comment)
    {
        var post = GetById(postId);
        comment.User = user;
        post.Comments.Add(comment);
        user.Comments.Add(comment);
        context.SaveChanges();

        return comment;
    }

    public Post Like(User user, int postId)
    {
        var post = FindPostFromUser(user, postId);
        ++post.Likes;

        return post;
    }   
    
    public Post Dislike(User user, int postId)
    {
        var post = FindPostFromUser(user, postId);
        --post.Likes;

        return post;
    }

    private IQueryable<Post> GetPosts()
    {
        return context.Posts
            .Include(p => p.Comments)
            .ThenInclude(c => c.User);   
    }

    private Post FindPostFromUser(User user, int postId)
    {
        return user.Posts.FirstOrDefault(p => p.Id == postId)
                ?? throw new EntityNotFoundException($"Post with id {postId} not found.");
    }

    public IEnumerable<Post> GetTopPosts()
    {
        return context.Posts
            .OrderByDescending(p => p.Comments)
            .Take(10)
            .Select(p => new Post { Id = p.Id, Title = p.Title, Comments = p.Comments })
            .ToList();
    }
}
