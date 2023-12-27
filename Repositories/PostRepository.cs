using PhotoForum.Data;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Repositories.Contracts;

public class PostRepository : IPostRepository
{
    private readonly PhotoForumContext context;
    private readonly Queue<Post> recentlyCreated;
    private readonly List<Post> topCommented;

    public PostRepository(PhotoForumContext context)
    {
        this.context = context;
        recentlyCreated = new Queue<Post>();
        topCommented = new List<Post>();
    }
    public Post Create(Post post)
    {
        recentlyCreated.Enqueue(post);
        if(recentlyCreated.Count > 10)
        {
            recentlyCreated.Dequeue();
        }
        
        context.Posts.Add(post);
        context.SaveChanges();

        return post;
    }
    public IList<Post> GetAll()
    {
        return GetPosts().ToList();
    }
    Post GetById(int id)
    {
        var post = GetPosts().FirstOrDefault(p => p.Id == id);

        return post ?? throw new EntityNotFoundException($"Post with id {id} not found.");
    }
    public Post Update(Post post, int id)
    {
        var postToUpdate = GetById(id);

        postToUpdate.User = post.User;
        postToUpdate.Title = post.Title;
        postToUpdate.Content = post.Content;
        postToUpdate.Likes = post.Likes;

        context.Update(postToUpdate);
        context.SaveChanges();

        return postToUpdate;
    }
    public Post Delete(int id)
    {
        var postToDelete = context.Users.FirstOrDefault(p => p.Id == id)
                ?? throw new EntityNotFoundException($"Post with id {id} not found.");
        RemoveFromQueue(postToDelete);
        topCommented.Remove(postToDelete);
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
        postToEdit.Likes = editedPost.Likes;

        context.Update(postToEdit);
        context.SaveChanges();

        return postToEdit;
    }
    public Post Comment(User user, int postId, Comment comment)
    {
        var post = FindPostFromUser(user, postId);
        post.Comments.Add(comment);
        RankingCheck(post);
        return post;
    }
    public Post Like(User user, int postId)
    {
        var post = FindPostFromUser(user, postId);
        ++post.Likes;

        return post;
    }
    private IQueryable<Post> GetPosts()
    {
        return context.Posts;
    }
    private Post FindPostFromUser(User user, int postId)
    {
        var userPosts = user.Posts.ToList();

        return userPosts.FirstOrDefault(p => p.Id == postId)
                ?? throw new EntityNotFoundException($"Post with id {postId} not found.");
    }
    private void RemoveFromQueue(Post elementToRemove)
    {
        Queue<Post> temporaryQueue = new Queue<Post>;

        while (recentlyCreated.Count > 0)
        {
            if(recentlyCreated.Peek() == elementToRemove)
            {
                recentlyCreated.Dequeue();
                break;
            }
            temporaryQueue.Enqueue(recentlyCreated.Dequeue());
        }

        while(recentlyCreated.Count > 0)
        {
            temporaryQueue.Enqueue(recentlyCreated.Dequeue());
        }
    }
    private void RankingCheck(Post postToCheck)
    {
        if (topCommented.Count == 10)
        {
            Post post = topCommented[10];
            if(postToCheck.Comments > post.Comments)
            {
                topCommented.Remove(post);
                topCommented.Add(postToCheck);
            }
        }
        else
        {
            topCommented.Add(postToCheck);
        }
        topCommented = topCommented.SortBy(c => c.Comments.Count.ToList());
    }
}
