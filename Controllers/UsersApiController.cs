using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Exceptions;
using PhotoForum.Helpers;
using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Services;
using PhotoForum.Services.Contracts;

namespace PhotoForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
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

        // GET: api/user/posts/id
        [HttpGet("posts/{id}")]
        public IActionResult GetPost(int id)
        {
            try
            {
                var post = postService.GetById(id);
                PostResponseDto postResponseDto = modelMapper.Map(post);

                return Ok(postResponseDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {id} not found.");
            }            
        }

        // POST: api/user/posts
        [HttpPost("posts")]
        public IActionResult CreatePost([FromBody] PostDTO dto)
        {
            Post post = modelMapper.Map(dto);

            Post createdPost = postService.Create(post);
            PostResponseDto createdPostDto = modelMapper.Map(createdPost);

            return StatusCode(StatusCodes.Status201Created, createdPostDto);
        }

        // PUT: api/user/posts/id
        [HttpPut("posts/{id}")]
        public IActionResult UpdatePost(int id, [FromHeader] string username, [FromBody] PostDTO dto)
        {

            try
            {
                Post post = modelMapper.Map(dto);
                if (username == post.User.Username)
                {
                    var user = usersService.GetUserByUsername(username);
                    Post updatedPost = postService.EditPost(user, id, post);
                    PostResponseDto postResponseDto = modelMapper.Map(updatedPost);
                    return Ok(postResponseDto);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
