using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoForum.Models
{
    public class Admin : BaseUser
    {
        public string? PhoneNumber { get; set; }
    }
}
