using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Admin : BaseUser
    {
        public string? PhoneNumber { get; set; }
    }
}
