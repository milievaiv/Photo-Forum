using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.Contracts;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Helpers
{
    public class ModelMapper : IModelMapper
    {
        public Post Map(User user, PostDTO dto)
        {
            return new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                Creator = user                
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
