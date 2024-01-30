using PhotoForum.Models;
using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class PostModel
    {
        [Required]
        [StringLength(64, MinimumLength = 16)]
        public string Title { get; set; }
        [Required]
        [StringLength(8192, MinimumLength = 32)]
        public string Content { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
