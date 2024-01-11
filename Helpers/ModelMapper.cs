using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Helpers
{
    public class ModelMapper : IModelMapper
    {
        public User Map(UserProfileUpdateModel model)
        {
            return new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
        }
        public UserResponseAndPostDto MapURPD(User user)
        {
            return new UserResponseAndPostDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsBlocked = user.IsBlocked,
                IsDeleted = user.IsDeleted,
                Posts = user.Posts
            };
        }

        public UserResponseDto Map(User user)
        {
            return new UserResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public Post Map(User user, PostDTO dto)
        {
            return new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                Creator = user,
                PhotoUrl = dto.PhotoUrl,
            };
        }

        public PostResponseDto Map(User user, Post postModel)
        {
            return new PostResponseDto()
            {
                Title = postModel.Title,
                Content = postModel.Content,
                Creator = user.Username,
                Likes = postModel.Likes,
                Comments = postModel.Comments?
                    .GroupBy(comment => comment.User.Username)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(comment => comment.Content).ToList()
                    ) ?? new Dictionary<string, List<string>>(),
                Tags = postModel.Tags?
                    .Select(tag => tag.Name)
                    .ToList() ?? new List<string>()

            };
        }

        public PostResponseDtoAndId Map(Post postModel)
        {
            return new PostResponseDtoAndId()
            {
                Id = postModel.Id,
                Title = postModel.Title,
                Content = postModel.Content,
                Creator = postModel.Creator.Username,
                Likes = postModel.Likes,
                Comments = postModel.Comments?
                    .GroupBy(comment => comment.User.Username)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(comment => comment.Content).ToList()
                    ) ?? new Dictionary<string, List<string>>()
            };
        }

        public Comment Map(CommentCreationDTO dto)
        {
            return new Comment
            {
                Content = dto.Content
            };
        }

        public CommentResponseDTO Map(Comment commentModel, User user)
        {
            return new CommentResponseDTO()
            {
                Creator = user.Username,
                Content = commentModel.Content
            };
        }

        //public Admin Map(AdminDTO dto)
        //{
        //    return new Admin()
        //    {
        //        Id = dto.User.Id,

        //    };
        //}
    }
}
