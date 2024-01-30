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
using PhotoForum.Helpers;
using System.Security.Claims;
using PhotoForum.Helpers.Contracts;
using PhotoForum.Models.QueryParameters;

namespace PhotoForum.Controllers
{
    [Route("api/admin")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAdminsService adminService;
        private readonly IPostService postService;
        private readonly IModelMapper modelMapper;
        public AdminApiController(IUsersService usersService, IAdminsService adminService, IModelMapper modelMapper, IPostService postService)
        {
            this.usersService = usersService;
            this.adminService = adminService;
            this.modelMapper = modelMapper;
            this.postService = postService;
        }

        [HttpPost("register")]
        public IActionResult RegisterAdmin([FromBody] RegisterAdminModel registerModel)
        {
            try
            {
                var user = adminService.Register(registerModel);
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
                UserResponseAndPostDto user = modelMapper.MapURPD(usersService.GetUserByUsername(username));
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
                UserResponseAndPostDto user = modelMapper.MapURPD(usersService.GetUserByEmail(email));
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
        public ActionResult<User> DeleteUser([FromQuery(Name = "username")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                User user = usersService.GetUserByUsername(username);
                adminService.Delete(user.Id);
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
        public ActionResult<User> BlockUser([FromQuery(Name = "username")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                adminService.Block(username);
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
        public ActionResult<User> UnblockUser([FromQuery(Name = "username")] string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username cannot be empty");
            }

            try
            {
                adminService.Unblock(username);
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

                IList<UserResponseAndPostDto> result = usersService
                    .FilterBy(filterParameters)                  
                    .Select(user => modelMapper.MapURPD(user))
                    .ToList();

                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No users found with the specified criteria.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                if (ex is InvalidOperationException) return BadRequest(ex.Message);
                else return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("posts")]
        public IActionResult FilterPosts([FromQuery] PostQueryParameters filterParameters)
        {
            IList<PostResponseDtoAndId> posts = postService.FilterBy(filterParameters).Select(post => modelMapper.Map(post)).ToList();
            if (posts.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(posts);
            }
        }

        [HttpGet("post")]
        public IActionResult GetPostById([FromQuery(Name = "id")] int id)
        {
            try
            {
                Post post = postService.GetById(id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException) return NotFound(ex.Message);
                else return BadRequest(ex.Message);
            }
            
        }

    }
}
