using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUsersService usersService;
        public UsersApiController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //GET: api/users
        [HttpGet("")]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = usersService
                .GetAll()
                .ToList();

            return Ok(users);
        }
    }
}
