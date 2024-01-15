using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models.ViewModel
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        public string ConfirmPassword { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
