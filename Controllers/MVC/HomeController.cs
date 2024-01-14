using Microsoft.AspNetCore.Mvc;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ITokenService tokenService;
        private readonly IVerificationService verificationService;

        public HomeController(ITokenService tokenService, IUsersService usersService, IVerificationService verificationService)
        {
            this.tokenService = tokenService;
            this.usersService = usersService;
            this.verificationService = verificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
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
