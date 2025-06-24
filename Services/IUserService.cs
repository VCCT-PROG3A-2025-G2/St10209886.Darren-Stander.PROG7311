using TestApp.Models;


namespace testApp.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        // Add other user-related methods as needed
    }
}