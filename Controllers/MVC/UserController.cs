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
using PhotoForum.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PhotoForum.Helpers.Contracts;

namespace PhotoForum.Controllers.MVC
{
    [AuthorizeRoles("user")]
    public class UserController : Controller
    {
        private readonly IPostService postService;
        private readonly IUsersService usersService;
        private readonly IModelMapper modelMapper;
        public UserController(IPostService postService, IUsersService usersService, IModelMapper modelMapper)
        {
            this.postService = postService;
            this.usersService = usersService;
            this.modelMapper = modelMapper;
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
            
            return View("UserPosts", posts); 
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var jwtFromRequest = Request.Cookies["Authorization"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
            var username = jwtToken.Payload[ClaimTypes.Name] as string;
            var user = usersService.GetUserByUsername(username);

            var model = modelMapper.MapUserProfile(user);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserProfile model)
        {
            if (ModelState.IsValid)
            {
                bool updateSuccessful = usersService.UpdateUserProfile(model);

                if (updateSuccessful)
                {
                    return RedirectToAction("ProfileUpdated");
                }
                else
                {
                    ModelState.AddModelError("", "Profile update failed.");
                }
            }

            return View(model);
        }
        public ActionResult ProfileUpdated()
        {
            return View();
        }
    }
}
