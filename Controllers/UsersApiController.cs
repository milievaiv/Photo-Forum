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

        // GET: api/users/posts/id
        [HttpGet("posts/{id}")]
        public IActionResult GetPost(int id)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = usersService.GetUserByUsername(username);
                var post = postService.GetById(id);
                PostResponseDto postResponseDto = modelMapper.Map(user,post);

                return Ok(postResponseDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {id} not found.");
            }            
        }

        // POST: api/users/posts
        [HttpPost("posts")]
        public IActionResult CreatePost([FromBody] PostDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = usersService.GetUserByUsername(username);
            
            Post post = modelMapper.Map(user,dto);

            Post createdPost = postService.Create(post);
            PostResponseDto createdPostDto = modelMapper.Map(user,createdPost);
           

            return StatusCode(StatusCodes.Status201Created, createdPostDto);
        }

        // PUT: api/users/posts/id
        [HttpPut("posts/{id}")]
        public IActionResult UpdatePost(int id, [FromBody] PostDTO dto)
        {

            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
                var user = usersService.GetUserByUsername(username);
                Post post = modelMapper.Map(user, dto);

                if (user.Id == post.User.Id)
                {
                    Post updatedPost = postService.EditPost(user, id, post);
                    PostResponseDto postResponseDto = modelMapper.Map(user,updatedPost);
                    return Ok(postResponseDto);
                }
                else
                {
                    return NotFound("Invalid operation! You are not the creator of the post!");
                }

            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {id} not found.");
            }
        }

        // POST: api/users/posts/id/comments
        [HttpPost("posts/{id}/comments")]
        public IActionResult CommentOnPost(int id, [FromBody] CommentCreationDTO dto)
        {
            try
            {
                if (postService.GetById(id) == null)
                {
                    return NotFound($"Post with id: {id} not found.");
                }

                var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
                var user = usersService.GetUserByUsername(username);
                
                Comment comment = modelMapper.Map(dto);
                Comment createdComment = postService.Comment(user, id, comment);
                CommentResponseDTO createdCommentDto = modelMapper.Map(createdComment, user);

                return StatusCode(StatusCodes.Status201Created, createdCommentDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {id} not found.");
            }
        }
    }
}
