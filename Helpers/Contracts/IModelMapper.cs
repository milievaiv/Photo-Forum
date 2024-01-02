using PhotoForum.Models.DTOs;
using PhotoForum.Models;

namespace PhotoForum.Helpers.Contracts
{
    public interface IModelMapper
    {
        Post Map(PostDTO dto);
        PostResponseDto Map(Post postModel);

        //PostCreationDto MapPCD(Post postModel);
    }
}
