using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories;
using PhotoForum.Data;
using PhotoForum.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;
using PhotoForum.Exceptions;

namespace PhotoForum.Controllers
{
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAdminsService _adminService;

        public AdminApiController(IUsersService usersService, IAdminsService adminService)
        {
            _usersService = usersService;
            _adminService = adminService;
        }

        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] RegisterAdminModel registerModel)
        {
            try
            {
                var user = _adminService.Register(registerModel);
                return Ok(user);
            }
            catch (DuplicateEntityException)
            {

                return Conflict("That username is taken.Try another.");
            }
        }

        [HttpGet("user/username")]
        public ActionResult<User> GetUserByUsername([FromQuery(Name = "value")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                User user = _usersService.GetUserByUsername(username);
                return Ok(user);

            }
            catch
            {
                return NotFound();
            }

        }

        [HttpGet("user/email")]
        public ActionResult<User> GetUserByEmail([FromQuery(Name = "value")] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be empty");
            }

            try
            {
                User user = _usersService.GetUserByEmail(email);
                return Ok(user);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpGet("user/firstname")]
        public ActionResult<User> GetUserByFirstName([FromQuery(Name = "value")] string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return BadRequest("First name cannot be empty");
            }

            try
            {
                User user = _usersService.GetUserByFirstName(firstName);
                return Ok(user);
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpPut("block")]
        public ActionResult<User> BlockUser([FromQuery(Name = "value")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                _usersService.Block(username);
                return Ok($"User {username} successfully suspended.");
            }
            catch
            {
                return NotFound();
            }

        }

        [HttpPut("unblock")]
        public ActionResult<User> UnblockUser([FromQuery(Name = "value")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                _usersService.Unblock(username);
                return Ok($"User {username} successfully unsuspended.");
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut("upgrade")]
        public ActionResult<User> UpgradeUserToAdmin([FromBody] AdminDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                _usersService.UpgradeToAdmin(dto.Username, dto.PhoneNumber);
                return Ok($"User {dto.Username} successfully upgraded to admin.");
            }
            catch
            {
                return NotFound();
            }
        }
        //public IActionResult SearchUsers()
        //{
        //    // Implement logic to search users based on the provided parameters
        //    // ...

        //    return Ok("Search results");
        //}

    }
}
