﻿using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.QueryParameters;

namespace PhotoForum.Services.Contracts
{
    public interface IPostService
    {
        Post Create(User user, Post post, List<Tag> tags);
        IList<Post> GetAll();
        Post GetById(int id);
        IList<Post> FilterBy(PostQueryParameters filterParameters);
        IList<Post> SearchBy(string filter);
        bool Delete(int id);
        IList<Post> GetUsersPost(User user);
        Post EditPost(User user, int postId, Post editedPost);
        Comment Comment(User user, int postId, Comment comment);
        Post Like(User user, int postId);
        Post Dislike(User user, int postId);
        IList<Post> GetTopPosts();
        IList<Post> RecentlyCreated();
    }
}
