using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        post.Creator = user;
        post.Date = DateTime.Now;
        context.Posts.Add(post);
        post.Creator.Posts.Add(post);
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
    
    public IList<Post> FilterBy(PostQueryParameters filterParameters)
    {
        IQueryable<Post> result = GetPosts();

        result = FilterByTitle(result, filterParameters.Title);
        result = FilterByContent(result, filterParameters.Content);
        result = FilterByCreator(result, filterParameters.Creator);
        result = SortBy(result, filterParameters.SortBy);

        return result.ToList();
    }

    private static IQueryable<Post> FilterByTitle(IQueryable<Post> posts, string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            return posts.Where(post => post.Title.Contains(title));
        }
        else
        {
            return posts;
        }
    }

    private static IQueryable<Post> FilterByContent(IQueryable<Post> posts, string Content)
    {
        if (!string.IsNullOrEmpty(Content))
        {
            return posts.Where(post => post.Content.Contains(Content));
        }
        else
        {
            return posts;
        }
    }

    private static IQueryable<Post> FilterByCreator(IQueryable<Post> posts, string creator)
    {
        if (!string.IsNullOrEmpty(creator))
        {
            return posts.Where(post => post.Creator.Username == creator);
        }
        else
        {
            return posts;
        }
    }

    private static IQueryable<Post> SortBy(IQueryable<Post> posts, string sortCriteria)
    {
        switch (sortCriteria)
        {
            case "likes":
                return posts.OrderBy(post => post.Likes);
            case "date":
                return posts.OrderBy(post => post.Date);
            case "title":
                return posts.OrderBy(post => post.Title);
            case "creator":
                return posts.OrderBy(post => post.Creator);
            default:
                return posts;
        }
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

    public IList<Post> GetTopPosts()
    {
        return context.Posts
            .OrderByDescending(p => p.Comments.Count)
            .Take(10)
            .Select(p => new Post { Id = p.Id, Title = p.Title, Comments = p.Comments })
            .ToList();
    }
    public IList<Post> RecentlyCreated()
    {
        return context.Posts
            .OrderByDescending(p => p.Date)
            .Take(10)
            .Select(p => new Post { Id = p.Id, Title = p.Title, Comments = p.Comments, Date = p.Date})
            .ToList();
    }
}
