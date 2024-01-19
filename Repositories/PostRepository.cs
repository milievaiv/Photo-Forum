using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;

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

    public Post Create(User user, Post post, List<Tag> tags)
    {
        post.Creator = user;
        post.Date = DateTime.Now;
        context.Posts.Add(post);
        post.Creator.Posts.Add(post);
        context.SaveChanges();
        //IMPORTANT USE LIST 
        //List<Tag> tags = new List<Tag>() { new Tag { Name = "one"}, new Tag { Name = "two" }, new Tag { Name = "three" } };
        CreateTag(post, tags);
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
        IList<Post> result = GetPosts().ToList();

        result = FilterByTitle(result, filterParameters.Title);
        result = FilterByContent(result, filterParameters.Content);
        result = FilterByCreator(result, filterParameters.Creator);
        result = FilterByTag(result, filterParameters.Tags);
        result = SortBy(result, filterParameters.SortBy);

        return result.ToList();        
    }
    public IList<Post> SearchBy(string filter)
    {
        //var posts = context.Posts
        // .FromSqlRaw($"Select * from Posts where Title like '%{filter}%' or Content like '%{filter}%' or Comments like '%{filter}%' or Tags like '%{filter}%'")
        //.ToList();
        var posts = context.Posts
            .Where(p => p.Title.Contains(filter) ||
                        p.Creator.Username.Contains(filter) ||
                        p.Content.Contains(filter) ||
                        p.Comments.Any(c => c.Content.Contains(filter)) ||
                        p.Tags.Any(t => t.Name.Contains(filter)))
            .Include(p => p.Comments)
            .Include(p => p.Tags)
            .ToList();

        return posts;
    }

    private IList<Post> FilterByTag(IList<Post> posts, string tag)
    {
        
        if (!string.IsNullOrEmpty(tag))
        {
            var searchedTag = FindTagByName(tag);
            return posts.Where(post => post.Tags.Contains(searchedTag)).ToList();
        }
        else
        {
            return posts;
        }
    }

    private static IList<Post> FilterByTitle(IList<Post> posts, string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            return posts.Where(post => post.Title.Contains(title)).ToList();
        }
        else
        {
            return posts;
        }
    }

    private static IList<Post> FilterByContent(IList<Post> posts, string Content)
    {
        if (!string.IsNullOrEmpty(Content))
        {
            return posts.Where(post => post.Content.Contains(Content)).ToList();
        }
        else
        {
            return posts;
        }
    }

    private static IList<Post> FilterByCreator(IList<Post> posts, string creator)
    {
        if (!string.IsNullOrEmpty(creator))
        {
            return posts.Where(post => post.Creator.Username == creator).ToList();
        }
        else
        {
            return posts;
        }
    }

    private static IList<Post> SortBy(IList<Post> posts, string sortCriteria)
    {
        switch (sortCriteria)
        {
            case "likes":
                return posts.OrderBy(post => post.Likes).ToList();
            case "date":
                return posts.OrderBy(post => post.Date).ToList();
            case "title":
                return posts.OrderBy(post => post.Title).ToList();
            case "creator":
                return posts.OrderBy(post => post.Creator).ToList();
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
        var like = new Like { UserId = user.Id, PostId = postId };
        context.Likes.Add(like);
        context.SaveChanges();

        var post = FindPostFromUser(user, postId);
        ++post.LikesCount;
        context.SaveChanges();

        return post;
    }   
    
    public Post Dislike(User user, int postId)
    {
        var like = context.Likes.Find(user.Id, postId);

        if (like != null)
        {
            context.Likes.Remove(like);
            context.SaveChanges();
        }

        var post = FindPostFromUser(user, postId);
        --post.LikesCount;
        context.SaveChanges();

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
            .Select(p => new Post { Id = p.Id, Title = p.Title, Comments = p.Comments, PhotoUrl = p.PhotoUrl })
            .ToList();
    }
    public IList<Post> RecentlyCreated()
    {
        return context.Posts
            .OrderByDescending(p => p.Date)
            .Take(10)
            .Select(p => new Post { Id = p.Id, Title = p.Title, Comments = p.Comments, Date = p.Date, PhotoUrl = p.PhotoUrl})
            .ToList();
    }
    private IQueryable<Tag> IQ_GetTags()
    {
        return context.Tags;
    }

    private IList<Tag> GetTags()
    {
        return IQ_GetTags().ToList();
    }

    public void CreateTag(Post post, List<Tag> tags)
    {
        foreach (var tag in tags)
        {
            // Check for duplicates and correct formatting
            if (tags.Any(existingTag => existingTag != tag && string.Equals(existingTag.Name, tag.Name)))
            {
                // Handle duplicate or incorrectly formatted tag
                throw new DuplicateEntityException($"Duplicate tag: {tag.Name}");
            }

            if (tag.Name.Any(char.IsUpper) || tag.Name.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                // Handle tags with uppercase letters or special symbols
                throw new Exception($"Invalid format for tag: {tag.Name}");
            }

            var _tag = GetTagByName(tag.Name);
            // Check if the tag exists in the database
            if (_tag == null)
            {
                context.Tags.Add(tag);
                context.SaveChanges();
                _tag = GetTagByName(tag.Name);
            }
            _tag.Posts.Add(post);
            post.Tags.Add(_tag);
            context.SaveChanges();
        }
    }

    public Tag GetTagByName(string name)
    {
        var tag = IQ_GetTags().FirstOrDefault(u => u.Name == name);
        context.SaveChanges();
        return tag;
    }
    private Tag FindTagByName(string tagName)
    { 
        var tag = GetTags().FirstOrDefault(u => u.Name == tagName);
        return tag;
    }
}
