using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.DTOs
{
    public class PasswordDTO
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
