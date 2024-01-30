using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models.DTOs;
using PhotoForum.Repositories.Contracts;
using PhotoForum.Services.Contracts;
using PhotoForum.Exceptions;

namespace PhotoForum.Controllers
{
    [ApiController]
    [Route("api")]
    public class AnonymousApiController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IPostService postService;
        public AnonymousApiController(IUsersService usersService, IPostService postService)
        {
            this.usersService = usersService;
            this.postService = postService;
        }
        [HttpGet("home")]
        public IActionResult HomePage()
        {
            int users = usersService.GetUsers().Count();
            int posts = postService.GetAll().Count();
            return Ok($"Core features of the platform: ..., \nNumber of users: {users}, \nNumber of posts: {posts}");
        }

        [HttpGet("top-comments")]
        public IActionResult TopComments()
        {
            return Ok(postService.GetTopPosts());
        }

        [HttpGet("recently-created")]
        public IActionResult RecentlyCreated()
        {
            return Ok(postService.RecentlyCreated());
        }
    }
}
