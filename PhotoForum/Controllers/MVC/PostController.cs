using Microsoft.AspNetCore.Mvc;
using PhotoForum.Models;
using PhotoForum.Services.Contracts;
using PhotoForum.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PhotoForum.Helpers;
using PhotoForum.Helpers.Contracts;
using PhotoForum.Exceptions;
using PhotoForum.Attributes;
using PhotoForum.Models.ViewModel.UserViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Update;

namespace PhotoForum.Controllers
{
    [AuthorizeRoles("user")]
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IPostService postService;
        private readonly IUsersService usersService;
        private readonly IModelMapper modelMapper;

        public PostController(IPostService postService, IWebHostEnvironment webHostEnvironment, IUsersService usersService, IModelMapper modelMapper)
        {
            this.postService = postService;
            this._webHostEnvironment = webHostEnvironment;
            this.usersService = usersService;
            this.modelMapper = modelMapper;
        }

        public IActionResult Index(int id)
        {
            var jwtFromRequest = Request.Cookies["Authorization"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
            var username = jwtToken.Payload[ClaimTypes.Name] as string;
            var user = usersService.GetUserByUsername(username);

            var post = this.postService.GetById(id);

            if (post == null)
            {
                return NotFound();
            }

            PostViewModel viewModel = new PostViewModel
            {
                Post = post,
                User = user

            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Like(int userId, int postId)
        {
            var user = usersService.GetUserById(userId);

            var post = postService.Like(user, postId);

            PostViewModel viewModel = new PostViewModel
            {
                Post = post,
                User = user

            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Dislike(int userId, int postId)
        {
            var user = usersService.GetUserById(userId);

            var post = postService.Dislike(user, postId);

            PostViewModel viewModel = new PostViewModel
            {
                Post = post,
                User = user

            };

            return View("Index", viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PostModel model = new PostModel();

            return View("Create", model);
        }

        [HttpPost]
        public IActionResult AddComment(int postId, string commentText)
        {
            var jwtFromRequest = Request.Cookies["Authorization"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
            var username = jwtToken.Payload[ClaimTypes.Name] as string;
            var user = usersService.GetUserByUsername(username);

            Comment comment = new Comment
            {
                Content = commentText,
                User = user
            };

            postService.Comment(user, postId, comment);

            var updatedPost = postService.GetById(postId);

            PostViewModel viewModel = new PostViewModel
            {
                Post = updatedPost,
                User = user

            };

            return View("Index", viewModel);
        }


        //[HttpPost]
        //public IActionResult LikePost(int postId)
        //{
        //    var jwtFromRequest = Request.Cookies["Authorization"];

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
        //    var username = jwtToken.Payload[ClaimTypes.Name] as string;
        //    var user = usersService.GetUserByUsername(username);

        //    var updatedPost = postService.GetById(postId);

        //    return View("Index", updatedPost);
        //}

        [HttpPost]
        public IActionResult Create([FromForm] PostModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var jwtFromRequest = Request.Cookies["Authorization"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(jwtFromRequest) as JwtSecurityToken;
                var username = jwtToken.Payload[ClaimTypes.Name] as string;
                //var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var user = usersService.GetUserByUsername(username);

                PostDTO dto = new PostDTO();
                UploadFile(model.Image);
                var imagesDirectory = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images"));

                // Get the last added file
                var image = GetLastAddedFile(imagesDirectory);

                if (ModelState.IsValid)
                {
                    dto.Title = model.Title;
                    dto.Content = model.Content;
                    dto.PhotoUrl = "/images/" + image.Name;


                    if (user.IsBlocked == true) throw new InvalidOperationException("You've been suspended from doing this.");
                    //List<Tag> tags = new List<Tag>() { new Tag { Name = "one"}, new Tag { Name = "two" }, new Tag { Name = "three" } };
                    List<Tag> tags = new List<Tag>();

                    foreach (string tag in dto.Tags)
                    {
                        tags.Add(new Tag { Name = tag });
                    }

                    Post post = modelMapper.Map(user, dto);

                    Post createdPost = postService.Create(user, post, tags);
                    PostResponseDto createdPostDto = modelMapper.Map(user, createdPost);

                    return RedirectToAction("Index", new { id = createdPost.Id });
                }
                else
                {
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<bool> UploadFile(IFormFile ufile)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var imagesDirectory = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images"));
                // Get the last added file
                var lastAddedFile = GetLastAddedFile(imagesDirectory);

                string lastFileName = "0";

                if (lastAddedFile != null) lastFileName = Path.GetFileNameWithoutExtension(lastAddedFile.Name);

                string newFileName = (int.Parse(lastFileName) + 1).ToString();
                // Get the file extension
                string fileExtension = Path.GetExtension(ufile.FileName);

                // Combine the edited file name with the original extension
                string newFilePath = Path.Combine(newFileName + fileExtension);

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newFilePath);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                return true;
            }
            return false;
        }

        public IActionResult LastAddedImage()
        {
            // Path to the images folder in wwwroot
            var imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            // Get the DirectoryInfo for the images folder
            var imagesDirectory = new DirectoryInfo(imagesFolderPath);

            // Get the last added file
            var lastAddedFile = GetLastAddedFile(imagesDirectory);

            // Pass the last added file to the view
            return View(lastAddedFile);
        }

        private FileInfo GetLastAddedFile(DirectoryInfo directory)
        {
            try
            {
                // Get the files in the directory, ordered by creation time
                var files = directory.GetFiles().OrderByDescending(f => f.CreationTimeUtc);

                // Return the first (i.e., the last added) file
                return files.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as directory not found
                // You may want to log the exception or take appropriate action
                return null;
            }
        }
    }
}
