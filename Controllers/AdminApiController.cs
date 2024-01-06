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
                _adminService.Block(username);
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
                _adminService.Unblock(username);
                return Ok($"User {username} successfully unsuspended.");
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> FilterUsers([FromQuery] UserQueryParameters filterParameters,
            [FromQuery(Name = "sortBy")] string sortBy)
        {
            try
            {
                // Call your service method with the provided filters

                IQueryable<User> users = _usersService.FilterBy(filterParameters);

                IList<User> result = new List<User>();

                if (!string.IsNullOrEmpty(sortBy))
                {
                    result = _usersService.SortBy(users, sortBy);
                }

                // Check if any users match the criteria
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
                // Log the exception for debugging purposes
                Console.WriteLine(ex);

                // Return a meaningful error response to the client
                return BadRequest("Invalid query parameters. Please provide at least one valid filter parameter.");
            }
        }

        //[HttpGet("users/sort")]
        //public ActionResult<IEnumerable<User>> SortUsers([FromQuery] UserQueryParameters filterParameters)
        //{
        //    try
        //    {
        //        // Call your service method with the provided filters

        //        IEnumerable<User> users = _usersService.SortBy(filterParameters);

        //        // Check if any users match the criteria
        //        if (users.Any())
        //        {
        //            return Ok(users);
        //        }
        //        else
        //        {
        //            return NotFound("No users found with the specified criteria");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Console.WriteLine(ex);

        //        // Return a meaningful error response to the client
        //        return BadRequest("Invalid query parameters. Please provide at least one valid filter parameter.");
        //    }
        //}

    }
}
