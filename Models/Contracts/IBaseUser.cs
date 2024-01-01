using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.Contracts
{
    public interface IBaseUser
    {
        public int Id { get; set; }
        public string Username { get; set; } 
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
