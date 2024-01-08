using PhotoForum.Models.DTOs;
using PhotoForum.Models;
using System.Runtime.ConstrainedExecution;

namespace PhotoForum.Helpers.Contracts
{
    public interface IModelMapper
    {
        User Map(UserProfileUpdateModel model);
        UserResponseDto Map(User user);
        Post Map(User user, PostDTO dto);
        PostResponseDto Map(User user,Post postModel);
        Comment Map(CommentCreationDTO dto);
        CommentResponseDTO Map(Comment commentModel, User user);

        //PostCreationDto MapPCD(Post postModel);
    }
}
