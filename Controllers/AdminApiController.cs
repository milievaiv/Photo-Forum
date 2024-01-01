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

namespace PhotoForum.Controllers
{
    [Route("api/admins")]
    [Authorize(Roles = "admin")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AdminApiController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            var user = _usersService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
