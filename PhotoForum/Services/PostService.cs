﻿using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.QueryParameters;
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

    public Post Create(User user, Post post, List<Tag> tags)
    {
        return postRepository.Create(user,post, tags);
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
    public IList<Post> FilterBy(PostQueryParameters filterParameters)
    {
        return postRepository.FilterBy(filterParameters);
    }
    public IList<Post> SearchBy(string filter)
    {
        return postRepository.SearchBy(filter);
    }
    public IList<Post> GetUsersPost(User user)
    {
        return postRepository.GetUsersPost(user);
    }

    public Post Like(User user, int postId)
    {
        return postRepository.Like(user, postId);
    }

    public Post Dislike(User user, int postId)
    {
        return postRepository.Dislike(user, postId);
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
