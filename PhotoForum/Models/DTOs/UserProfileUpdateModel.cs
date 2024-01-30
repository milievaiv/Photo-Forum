using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class UserProfileUpdateModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(32, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(4, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string FirstName { get; set; }

        [MaxLength(32, ErrorMessage = "The {0} field must be less than {1} characters.")]
        [MinLength(4, ErrorMessage = "The {0} field must be at least {1} character.")]
        public string LastName { get; set; }

    }
}
