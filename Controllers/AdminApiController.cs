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
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }

        }        

        [HttpPut("delete")]
        public ActionResult<User> DeleteUser([FromQuery(Name = "value")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                User user = _usersService.GetUserByUsername(username);
                _adminService.Delete(user.Id);
                return Ok($"User {username} successfully deleted.");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
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
                _adminService.Block(username);
                return Ok($"User {username} successfully suspended.");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
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
                _adminService.Unblock(username);
                return Ok($"User {username} successfully unsuspended.");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is EntityNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> FilterUsers([FromQuery] UserQueryParameters filterParameters)
        {
            try
            {
                if (AreAllUQPNullOrEmpty(filterParameters))
                {
                    throw new InvalidOperationException("Please provide at least one valid filter parameter.");
                }

                IList<User> result = _usersService.FilterBy(filterParameters);

                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No users found with the specified criteria");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                if (ex is InvalidOperationException) return BadRequest(ex.Message);
                else return BadRequest(ex.Message);
            }
        }
        private bool AreAllUQPNullOrEmpty(UserQueryParameters parameters)
        {
            return string.IsNullOrEmpty(parameters.FirstName)
                && string.IsNullOrEmpty(parameters.LastName)
                && string.IsNullOrEmpty(parameters.SortBy);
        }

    }
}
