using Microsoft.AspNetCore.Mvc;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Controllers.MVC
{
    public class AuthController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ITokenService tokenService;
        private readonly IVerificationService verificationService;

        public AuthController(ITokenService tokenService, IUsersService usersService, IVerificationService verificationService)
        {
            this.tokenService = tokenService;
            this.usersService = usersService;
            this.verificationService = verificationService;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies["Authorization"] != null)
            {
                Response.Cookies.Delete("Authorization");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginModel model = new LoginModel();

            return View("Login", model);
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginModel model)
        {
            LoginModel result = new LoginModel();
            if (ModelState.IsValid)
            {

                result = new LoginModel
                {
                    Username = model.Username,
                    Password = model.Password
                };
            }
            try
            {
                var user = verificationService.AuthenticateUser(result);
                string role = DetermineUserRole(user);
                string token = tokenService.CreateToken(user, role);                

                Response.Cookies.Append("Authorization", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                if (role == "admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index", "User");
            }
            catch (UnauthorizedOperationException)
            {
                return BadRequest("Invalid login attempt!");
            }
            catch (EntityNotFoundException)
            {
                return BadRequest("Invalid login attempt!");
            }
        }

        private string DetermineUserRole(BaseUser user)
        {
            if (user is Admin)
            {
                return "admin";
            }
            else
            {
                return "user";
            }
        }
    }
}
