using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhotoForum.Exceptions;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;
using System.Data.Entity.Validation;
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

        public AuthController(ITokenService tokenService, IUsersService usersService)
        {
            this.tokenService = tokenService;
            this.usersService = usersService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterModel registerModel)
        {
            try
            {
                var user = usersService.RegisterUser(registerModel);
                return Ok(user);
            }
            catch (DuplicateEntityException)
            {

                return Conflict("That username is taken.Try another.");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                var user = usersService.AuthenticateUser(loginModel);
                string token = tokenService.CreateToken(user);
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

    //    private string CreateToken(User user)
    //    {
    //        List<Claim> claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.Name, user.Username)
    //        };

    //        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
    //            configuration.GetSection("JwtConfig:Secret").Value));

    //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    //        var token = new JwtSecurityToken(
    //            claims: claims,
    //            expires: DateTime.Now.AddHours(1),
    //            signingCredentials: credentials);

    //        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    //        return jwt;
    //    }
    }
}
