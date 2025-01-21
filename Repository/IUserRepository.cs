using Blog.Models;

namespace Blog.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> RegisterUserAsync(User user);
        // Task<User> UpdateUserAsync(User user);
    }
}