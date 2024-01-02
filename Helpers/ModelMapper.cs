using PhotoForum.Helpers.Contracts;
using PhotoForum.Models;
using PhotoForum.Models.DTOs;

namespace PhotoForum.Helpers
{
    public class ModelMapper : IModelMapper
    {
        public Post Map(PostDTO dto)
        {
            return new Post
            {
                Title = dto.Title,
                Content = dto.Content
            };
        }
        public PostResponseDto Map(Post postModel)
        {
            return new PostResponseDto()
            {
                Title = postModel.Title,
                Content = postModel.Content,
                Creator = postModel.User.Username,
                Likes = postModel.Likes,
                Comments = postModel.Comments?.ToDictionary(r => r.User.Username, r => r.Content) ?? new Dictionary<string, string>()
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
