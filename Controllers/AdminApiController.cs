using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using PhotoForum.Repositories;
using PhotoForum.Data;
using PhotoForum.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using PhotoForum.Services;

namespace PhotoForum.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {

        private readonly UsersRepository usersRepository;

        public AdminApiController(UsersRepository _usersRepository)
        {
            usersRepository = _usersRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(int id)
        {
            var user = usersRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
