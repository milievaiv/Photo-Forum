using PhotoForum.Exceptions;
using PhotoForum.Models;

namespace PhotoForum.Helpers
{
    public static class ValidationHelper
    {
        public static void Exists(User user)
        {
            if (user == null || user.IsDeleted)
            {
                throw new EntityNotFoundException("User not found.");
            }
        }        
        public static void Blocked(User user)
        {
            if (user.IsBlocked == true)
            {
                throw new InvalidOperationException("You've been suspended from doing this.");
            }
        }
        public static void Compare(User user1, User user2)
        {
            if (user1.Id == user2.Id)
            {
                throw new InvalidOperationException("You can't do this!");
            }
        }
    }
}
