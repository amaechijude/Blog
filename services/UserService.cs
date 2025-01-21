
using Blog.Auth;
using Blog.DTOs;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Identity;

namespace Blog.services
{
    public class UserService(IUserRepository userRepository, TokenProvider tokenProvider) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly TokenProvider _tokenProvider = tokenProvider;

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

        public async Task<JwtUserViewDto> LoginUserAsync(LoginUserDTO loginUser)
        {
            if (string.IsNullOrWhiteSpace(loginUser.Email) || string.IsNullOrWhiteSpace(loginUser.Password))
                throw new ArgumentException("Email and Password are required");
            
            var user = await _userRepository.GetUserByEmailAsync(loginUser.Email) ?? throw new Exception ($"User with email {loginUser.Email} does not exist");

#pragma warning disable CS8604 // Possible null reference argument.
            var verifyLogin = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
#pragma warning restore CS8604 // Possible null reference argument.

            if (verifyLogin == PasswordVerificationResult.Failed)
                throw new Exception("Email or Password Incorrect");

            var token = _tokenProvider.Create(user);
            // generate JWT
            return new JwtUserViewDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                JwtToken = token
            };
            
        }
    }
}