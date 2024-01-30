using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoForum.Exceptions;
using PhotoForum.Helpers;
using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;
using PhotoForum.Models.QueryParameters;
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
                PostResponseDto postResponseDto = modelMapper.Map(user, post);

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
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = usersService.GetUserByUsername(username);

                if (user.IsBlocked == true) throw new InvalidOperationException("You've been suspended from doing this.");

                List<Tag> tags = new List<Tag>();

                foreach (string tag in dto.Tags)
                {
                    tags.Add(new Tag { Name = tag });
                }

                Post post = modelMapper.Map(user, dto);

                Post createdPost = postService.Create(user, post, tags);
                PostResponseDto createdPostDto = modelMapper.Map(user, createdPost);


                return StatusCode(StatusCodes.Status201Created, createdPostDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/users/posts/id
        [HttpPut("posts/{id}")]
        public IActionResult UpdatePost(int id, [FromBody] PostDTO dto)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = usersService.GetUserByUsername(username);

                Post post = modelMapper.Map(user, dto);

                Post updatedPost = postService.EditPost(user, id, post);
                PostResponseDto postResponseDto = modelMapper.Map(user, updatedPost);
                return Ok(postResponseDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound("Operation failed: The post does not exist, or you are not the creator of the post.");
            }
        }

        // POST: api/users/posts/id/comments
        [HttpPost("posts/{id}/comments")]
        public IActionResult CommentOnPost(int id, [FromBody] CommentCreationDTO dto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
            var user = usersService.GetUserByUsername(username);

            try
            {
                if (user.IsBlocked == true) throw new InvalidOperationException("You've been suspended from doing this.");
                if (postService.GetById(id) == null)
                {
                    return NotFound($"Post with id: {id} not found.");
                }
                Comment comment = modelMapper.Map(dto);
                Comment createdComment = postService.Comment(user, id, comment);
                CommentResponseDTO createdCommentDto = modelMapper.Map(createdComment, user);

                return StatusCode(StatusCodes.Status201Created, createdCommentDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {id} not found.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // DELETE: api/users/posts/id
        [HttpDelete("posts/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
                var user = usersService.GetUserByUsername(username);
                var post = postService.GetById(postId);

                if (post.Creator.Id != user.Id)
                {
                    return Conflict("You can only delete your own posts.");
                }
                else
                {
                    postService.Delete(postId);
                    return NoContent();
                }
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Post with id: {postId} not found.");
            }
        }

        //GET: api/users?filterParameter=filter
        [HttpGet("posts")]
        public IActionResult FilterPosts([FromQuery] PostQueryParameters filterParameters)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value; // Get the username from the JWT token
            var user = usersService.GetUserByUsername(username);

            List<PostResponseDto> posts = postService
                .FilterBy(filterParameters)
                .Select(post => modelMapper.Map(user, post))
                .ToList();
            if (posts.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(posts);
            }
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
