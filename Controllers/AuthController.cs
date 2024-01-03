using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PhotoForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                var user = usersService.Register(registerModel);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (DuplicateEntityException)
            {

                return Conflict("That username is taken.Try another.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = verificationService.AuthenticateUser(loginModel);
                string role = DetermineUserRole(user); // Implement this method to determine the role
                string token = tokenService.CreateToken(user, role);
                return Ok(token);
            }
            catch (UnauthorizedOperationException)
            {
                return BadRequest("Invalid login attempt!");
            }
            catch(EntityNotFoundException)
            { 
                return BadRequest("Invalid login attempt!");
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
