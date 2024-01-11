using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Exceptions;
using PhotoForum.Helpers;
using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;
using System.Security.Claims;

namespace PhotoForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IPostService postService;
        private readonly IModelMapper modelMapper;
        public UsersApiController(IUsersService usersService, IPostService postService, IModelMapper modelMapper)
        {
            this.usersService = usersService;
            this.postService = postService;
            this.modelMapper = modelMapper;
        }

        // PUT: api/users/userId/update
        [HttpPut("{userId}/update")]
        public IActionResult UpdateProfile([FromRoute]int userId, [FromBody] UserProfileUpdateModel model)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
                var user = usersService.GetUserByUsername(username);

                if (user.Id == userId)
                {
                    var userWithUpdateInfo = modelMapper.Map(model);
                    var updatedUser = usersService.Update(user.Id, userWithUpdateInfo);
                    var userResponseDto = modelMapper.Map(updatedUser);

                    return Ok(userResponseDto);
                }
                else
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed);
                }
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
