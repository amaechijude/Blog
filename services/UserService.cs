
using Blog.DTOs;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Identity;

namespace Blog.services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public async Task<UserProfileDTO?> GetUserByIdAsync(int Id)
        {
            return await _userRepository.GetUserByIdAsync(Id)
                ?? throw new KeyNotFoundException("User not found");
        }

        public async Task<UserProfileDTO?> RegisterUser(RegisterUserDTO registerUser)
        {
            if (string.IsNullOrWhiteSpace(registerUser.Email) || string.IsNullOrWhiteSpace(registerUser.Password))
                throw new ArgumentException("Email and Password are required");

            var userReg = new User
            {
                Email = registerUser.Email,
                FullName = registerUser.FullName,
                JoinedOn = DateTime.UtcNow
            };
            userReg.PasswordHash = _passwordHasher.HashPassword(userReg, registerUser.Password);

            var user = await _userRepository.RegisterUserAsync(userReg);
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                JoinedOn = user.JoinedOn
            };
            return userProfile;
        }
    }
}