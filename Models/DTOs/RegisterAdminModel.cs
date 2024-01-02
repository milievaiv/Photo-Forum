using PhotoForum.Models.Contracts;

namespace PhotoForum.Models.DTOs
{
    public class RegisterAdminModel : RegisterModel
    {
        public string ?PhoneNumber { get; set; }
        // Implement other properties for RegisterAdminModel
    }
}
