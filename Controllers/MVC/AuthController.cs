using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Controllers.Data.Exceptions;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.ViewModel;
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
                string role = DetermineUserRole(user); // Implement this method to determine the role
                string token = tokenService.CreateToken(user, role);                

                Response.Cookies.Append("Authorization", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                //Response.Cookies.Append("Role", role, new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.Strict
                //});

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

        [HttpGet]
        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel();

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

                RegisterModel model = new RegisterModel()
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
                _ = usersService.Register(model);

                return RedirectToAction("Login", "Auth");
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

        private string DetermineUserRole(BaseUser user)
        {
            // Implement logic to determine the role of the user (e.g., "user" or "admin")
            // You might use user.GetType() or any other criteria depending on your application
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
