using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;

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
    public Post Create(Post post)
    {   
        context.Posts.Add(post);

        context.SaveChanges();
        
        return post;
    }
    public IList<Post> GetAll()
    {
        return GetPosts().ToList();
    }
    private IQueryable<Post> GetPosts()
    {
        return context.Posts;
    }
    Post GetById(int id)
    {
        var post = GetPosts().FirstOrDefault(p => p.Id == id);

        return post ?? throw new EntityNotFoundException($"Post with id {id} not found.");
    }

    //public Post Update(Post post, int id)
    //{
    //    var postToUpdate = GetById(id);

    //    postToUpdate.Title = post.Title;
    //    postToUpdate.Content = post.Content;

    //    context.Update(postToUpdate);
    //    context.SaveChanges();

    //    return postToUpdate;
    //}

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
    public Post Comment(User user, int postId, Comment comment)
    {
        var post = FindPostFromUser(user, postId);
        post.Comments.Add(comment);
        return post;
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
    private Post FindPostFromUser(User user, int postId)
    {
        var userPosts = user.Posts.ToList();

        return userPosts.FirstOrDefault(p => p.Id == postId)
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
