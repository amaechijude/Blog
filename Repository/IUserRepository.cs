using Blog.DTOs;
using Blog.Models;

namespace Blog.Repository
{
    public interface IUserRepository
    {
        Task<UserProfileDTO> GetUserByIdAsync(int userId);
        Task<User> RegisterUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string userEmail);
    }
}