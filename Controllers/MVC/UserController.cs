using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using PhotoForum.Attributes;
using Microsoft.EntityFrameworkCore;
using PhotoForum.Services.Contracts;
using PhotoForum.Services;
using PhotoForum.Models;
using PhotoForum.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace PhotoForum.Controllers.MVC
{
    [AuthorizeRoles("user")]
    public class UserController : Controller
    {
        private readonly IPostService postService;
        private readonly IUsersService usersService;
        public UserController(IPostService postService, IUsersService usersService)
        {
            this.postService = postService;
            this.usersService = usersService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserPosts(string username)
        {
            var user = usersService.GetUserByUsernameWithPosts(username);
            var posts = postService.GetUsersPost(user);
            //var posts = _dbContext.Posts.Where(p => p.UserId == userId).ToList();
            return View("UserPosts", posts); // Assuming UserPosts.cshtml is your view
        }
    }
}
