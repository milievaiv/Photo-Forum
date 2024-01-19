using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.ViewModel;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostService postsService;
        private readonly ITokenService tokenService;
        private readonly IVerificationService verificationService;

        public HomeController(ITokenService tokenService, IUsersService usersService, IPostService postsService, IVerificationService verificationService)
        {
            this.postsService = postsService;
            this.tokenService = tokenService;
            this.usersService = usersService;
            this.verificationService = verificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                RecentPhotos = postsService.RecentlyCreated(),
                TopPhotos = postsService.GetTopPosts()
            };

            return View(viewModel);

            //SiteStatistics siteStatistics = new SiteStatistics
            //{
            //    Users_Count = users_count,
            //    Posts_Count = posts_count
            //};

            //return View("Index", siteStatistics);
        }

        //[HttpGet]
        //public IActionResult LogOut()
        //{

        //    return View("Index");
        //}

        [HttpGet]
        public IActionResult Auth()
        {
            return RedirectToAction("Login", "Auth");

        }
    }
}
