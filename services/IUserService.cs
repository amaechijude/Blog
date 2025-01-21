
using Blog.DTOs;

namespace Blog.services
{
    public interface IUserService
    {
        Task<UserProfileDTO?> RegisterUser(RegisterUserDTO registerUser);
        Task<UserProfileDTO?> GetUserByIdAsync(int id);
    }
}