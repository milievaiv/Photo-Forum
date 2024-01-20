using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using PhotoForum.Attributes;
using PhotoForum.Models;
using PhotoForum.Models.ViewModel.AdminViewModels;
using PhotoForum.Services.Contracts;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace PhotoForum.Controllers.MVC
{
    [AuthorizeRoles("admin")]
    public class AdminController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IAdminsService adminsService;
        private readonly IPostService postsService;

        public AdminController(IUsersService usersService, IAdminsService adminsService, IPostService postsService)
        {
            this.usersService = usersService;
            this.adminsService = adminsService;
            this.postsService = postsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterAdmin()
        {
            return View();
        }

        //Posts

        public IActionResult SearchPosts(PhotoForum.Models.ViewModel.AdminViewModels.PostViewModel viewModel)
        {
            //SearchUser searchUser = new SearchUser
            //{
            //    Property = viewModel.SearchModel.Property,
            //    Value = viewModel.SearchModel.Value
            //};
            //UserViewModel viewModel = new UserViewModel
            //{
            //    CurrentPage = page,
            //    PageSize = pageSize,
            //    SearchModel = searchUser
            //};
            viewModel = GeneratePostView(viewModel);

            return View("Posts", viewModel);
        }

        public IActionResult Posts(PostViewModel viewModel, int page = 1, int pageSize = 10, string Property = null, string Value = null)
        {
            // Update the SearchModel based on the provided parameters

            if (viewModel.SearchModel == null)
            {
                viewModel.SearchModel = new SearchPost { Property = Property, Value = Value };
            }

            viewModel = GeneratePostView(viewModel, page, pageSize);

            return View(viewModel);
        }
        public PostViewModel GeneratePostView(PostViewModel viewModel, int page = 1, int pageSize = 10)
        {
            string sortOrder = viewModel.SortOrder;
            string sortBy = viewModel.SortBy;

            PostQueryParameters postQueryParameters = new PostQueryParameters();

            switch (viewModel.SearchModel.Property)
            {
                case "Title":
                    postQueryParameters.Title = viewModel.SearchModel.Value;
                    break;
                case "Creator":
                    postQueryParameters.Creator = viewModel.SearchModel.Value;
                    break;
                    //case "FirstName":
                    //    postQueryParameters. = viewModel.SearchModel.Value;
                    //    break;
            }


            var filteredPosts = postsService.FilterBy(postQueryParameters);

            switch (sortBy)
            {
                case "Id":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Id).ToList() : filteredPosts.OrderByDescending(p => p.Id).ToList();
                    break;
                case "Creator":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Creator.Username).ToList() : filteredPosts.OrderByDescending(p => p.Creator.Username).ToList();
                    break;
                case "Title":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Title).ToList() : filteredPosts.OrderByDescending(p => p.Title).ToList();
                    break;
                case "Comments":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Comments.Count).ToList() : filteredPosts.OrderByDescending(p => p.Comments.Count).ToList();
                    break;
                case "Likes":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Likes).ToList() : filteredPosts.OrderByDescending(p => p.Likes).ToList();
                    break;
                case "Date":
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.Date).ToList() : filteredPosts.OrderByDescending(p => p.Date).ToList();
                    break;
                    // Add cases for other columns as needed
            }

            viewModel.Posts = filteredPosts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            // Update pagination information
            viewModel.CurrentPage = page;
            viewModel.PageSize = pageSize;
            viewModel.TotalPages = (int)Math.Ceiling(filteredPosts.Count / (double)pageSize);

            return viewModel;
        }

        public IActionResult DeletePost(PostViewModel viewModel, int post_id, int page, int pageSize = 10, string Property = null, string Value = null)
        {
            postsService.Delete(post_id);

            var jwtFromRequest = Request.Cookies["Authorization"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
            var username = jwtToken.Payload[ClaimTypes.Name] as string;

            adminsService.AddLog($"Admin {{ {username} }} has deleted a post with id {{ {post_id} }}."); 

            viewModel.CurrentPage = page;
            viewModel.PageSize = pageSize;

            viewModel.SearchModel = new SearchPost();

            viewModel.SearchModel.Property = Property;
            viewModel.SearchModel.Value = Value;
            // Perform blocking logic
            //SearchPost searchPost = new SearchPost
            //{
            //    Property = Property,
            //    Value = Value
            //};
            //PostViewModel viewModel = new PostViewModel
            //{
            //    CurrentPage = page,
            //    PageSize = pageSize,
            //    SearchModel = searchPost
            //};

            viewModel = GeneratePostView(viewModel, page, pageSize);

            return View("Posts", viewModel);
        }


        //Users

        [HttpPost]
        public IActionResult SearchUsers(UserViewModel viewModel)
        {
            //SearchUser searchUser = new SearchUser
            //{
            //    Property = viewModel.SearchModel.Property,
            //    Value = viewModel.SearchModel.Value
            //};
            //UserViewModel viewModel = new UserViewModel
            //{
            //    CurrentPage = page,
            //    PageSize = pageSize,
            //    SearchModel = searchUser
            //};
            viewModel = GenerateUserView(viewModel);

            return View("Users", viewModel);
        }

        public IActionResult Users(UserViewModel viewModel, int page = 1, int pageSize = 5, string Property = null, string Value = null)
        {
            // Update the SearchModel based on the provided parameters
            if (viewModel.SearchModel == null)
            {
                viewModel.SearchModel = new SearchUser { Property = Property, Value = Value };
            }

            viewModel = GenerateUserView(viewModel, page, pageSize);

            return View(viewModel);
        }

        public UserViewModel GenerateUserView(UserViewModel viewModel, int page = 1, int pageSize = 5)
        {
            UserQueryParameters userQueryParameters = new UserQueryParameters();

            switch (viewModel.SearchModel.Property)
            {
                case "Username":
                    userQueryParameters.Username = viewModel.SearchModel.Value;
                    userQueryParameters.SortBy = "username";
                    break;
                case "Email":
                    userQueryParameters.Email = viewModel.SearchModel.Value;
                    userQueryParameters.SortBy = "email";
                    break;
                case "FirstName":
                    userQueryParameters.FirstName = viewModel.SearchModel.Value;
                    userQueryParameters.SortBy = "firstname";
                    break;
            }

            var filteredUsers = usersService.FilterBy(userQueryParameters).ToList();

            viewModel.Users = filteredUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Update pagination information
            viewModel.CurrentPage = page;
            viewModel.PageSize = pageSize;
            viewModel.TotalPages = (int)Math.Ceiling(filteredUsers.Count / (double)pageSize);

            return viewModel;
        }

        [HttpPost]
        public IActionResult BlockUser(string username, int page, int pageSize = 5, string Property = null, string Value = null)
        {
            adminsService.Block(username);

            adminsService.AddLog($"Admin {{ {username} }} has blocked a user with id {{ {usersService.GetUserByUsername(username).Id} }}.");

            // Perform blocking logic
            SearchUser searchUser = new SearchUser
            {
                Property = Property,
                Value = Value
            };
            UserViewModel viewModel = new UserViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                SearchModel = searchUser
            };

            viewModel = GenerateUserView(viewModel, page, pageSize);

            return View("Users", viewModel);
        }

        [HttpPost]
        public IActionResult UnblockUser(string username, int page, int pageSize = 5, string Property = null, string Value = null)
        {
            adminsService.Unblock(username);

            adminsService.AddLog($"Admin {{ {username} }} has unblocked a user with id {{ {usersService.GetUserByUsername(username).Id} }}.");

            SearchUser searchUser = new SearchUser
            {
                Property = Property,
                Value = Value
            };
            UserViewModel viewModel = new UserViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                SearchModel = searchUser
            };

            viewModel = GenerateUserView(viewModel, page, pageSize);

            // Redirect back to the Users view with the updated data
            return View("Users", viewModel);
        }
    }
}
