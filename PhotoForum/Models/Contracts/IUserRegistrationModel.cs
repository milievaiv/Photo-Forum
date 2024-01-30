namespace PhotoForum.Models.Contracts
{
    public interface IUserRegistrationModel
    {
        string Username { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string Password { get; }
    }
}
