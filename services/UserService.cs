
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
            var user = await _userRepository.GetUserByIdAsync(Id) 
                ?? throw new KeyNotFoundException(" User does not exist");

            return new UserProfileDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                JoinedOn = user.JoinedOn
            };
        }

        public async Task<UserProfileDTO?> RegisterUser(RegisterUserDTO registerUser)
        {
            if (string.IsNullOrWhiteSpace(registerUser.Email) || string.IsNullOrWhiteSpace(registerUser.Password) || string.IsNullOrWhiteSpace(registerUser.Username))
                return null;

            var user = new User
            {
                Email = registerUser.Email,
                JoinedOn = DateTime.UtcNow
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, registerUser.Password);
            _context.Users.Add(user);

            // probably generate JWT token here
            await _context.SaveChangesAsync();
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                JoinedOn = user.JoinedOn,
                Posts = []
            };
            return userProfile;
        }

        public async Task<UserProfileDTO?> LoginUser(LoginUserDTO loginUser)
        {
            if (string.IsNullOrWhiteSpace(loginUser.Email) || string.IsNullOrWhiteSpace(loginUser.Password))
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
                return null;

            var result = PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            // probably generate JWT token here
            // probably generate JWT token here
            await _context.SaveChangesAsync();
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                JoinedOn = user.JoinedOn,
            };
            return userProfile;
        }


    }
}