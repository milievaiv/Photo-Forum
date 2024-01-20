using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using PhotoForum.Attributes;
using PhotoForum.Controllers.Data.Exceptions;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.QueryParameters;
using PhotoForum.Models.Search;
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

        public IActionResult Index(DashboardViewModel viewModel)
        {
            var jwtFromRequest = Request.Cookies["Authorization"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
            var username = jwtToken.Payload[ClaimTypes.Name] as string;

            var totalPosts = postsService.GetAll().Count();
            var totalUsers = usersService.GetUsers().Count();
            var topUser = usersService.GetUsers().OrderByDescending(user => user.Posts.Count).FirstOrDefault();
            var logs = adminsService.GetLastAddedLogs().ToList();
            var mostRecentPosts = postsService.GetAll().TakeLast(5).ToList();
            var admin = adminsService.GetAdminByUsername(username);

            viewModel = new DashboardViewModel
            {
                TotalPosts = totalPosts,
                TotalUsers = totalUsers,
                TopUser = topUser,
                Logs = logs,
                MostRecentPosts = mostRecentPosts,
                Admin = admin
            };

            return View("Index", viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel viewModel = new RegisterViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                if (viewModel.Password != viewModel.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");

                    return View(viewModel);
                }

                RegisterAdminModel model = new RegisterAdminModel()
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber
                };

                _ = adminsService.Register(model);


				var routeValues = new { Property = "Username", Value = model.Username };


				return RedirectToAction("Admins", routeValues);
            }
            catch (DuplicateEntityException)
            {
                ModelState.AddModelError("Username", "This username is already taken.");

                return View(viewModel);
            }
            catch (DuplicateEmailException)
            {
                ModelState.AddModelError("Email", "This email is already taken.");

                return View(viewModel);
            }
        }
        //Posts

        public IActionResult SearchPosts(PhotoForum.Models.ViewModel.AdminViewModels.PostViewModel viewModel)
        {
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
                    filteredPosts = sortOrder == "asc" ? filteredPosts.OrderBy(p => p.LikesCount).ToList() : filteredPosts.OrderByDescending(p => p.LikesCount).ToList();
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

        //Admins

        [HttpPost]
        public IActionResult SearchAdmins(AdminViewModel viewModel)
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
            viewModel = GenerateAdminView(viewModel);

            return View("Admins", viewModel);
        }

        public IActionResult Admins(AdminViewModel viewModel, int page = 1, int pageSize = 5, string Property = null, string Value = null)
        {
            // Update the SearchModel based on the provided parameters
            if (viewModel.SearchModel == null)
            {
                viewModel.SearchModel = new SearchUser { Property = Property, Value = Value };
            }

            viewModel = GenerateAdminView(viewModel, page, pageSize);

            return View(viewModel);
        }

        public AdminViewModel GenerateAdminView(AdminViewModel viewModel, int page = 1, int pageSize = 5)
        {
            AdminQueryParameters userQueryParameters = new AdminQueryParameters();

            switch (viewModel.SearchModel.Property)
            {
                case "Username":
                    userQueryParameters.Username = viewModel.SearchModel.Value;
                    break;
                case "Email":
                    userQueryParameters.Email = viewModel.SearchModel.Value;
                    break;
                case "FirstName":
                    userQueryParameters.FirstName = viewModel.SearchModel.Value;
                    break;                
                case "PhoneNumber":
                    userQueryParameters.FirstName = viewModel.SearchModel.Value;
                    break;
            }

            var filteredAdmins = adminsService.FilterBy(userQueryParameters).ToList();

            viewModel.Admins = filteredAdmins
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Update pagination information
            viewModel.CurrentPage = page;
            viewModel.PageSize = pageSize;
            viewModel.TotalPages = (int)Math.Ceiling(filteredAdmins.Count / (double)pageSize);

            return viewModel;
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
