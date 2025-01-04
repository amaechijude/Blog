using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class UserRepository(AppDbContext context)
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly AppDbContext _context = context;

        public async Task<UserProfileDTO?> GetUserById(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user is null)
                return null;
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Avatar = user.AvatarURL,
                JoinedOn = user.JoinedOn
            };
            return userProfile;
        }

        public async Task<UserProfileDTO?> RegisterUser(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.Password))
                return null;

            var user = new User
            {
                Email = userDTO.Email,
                JoinedOn = DateTime.Now
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.Password);

            _context.Users.Add(user);
            // probably generate JWT token here
            await _context.SaveChangesAsync();
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Avatar = user.AvatarURL,
                JoinedOn = user.JoinedOn
            };
            return userProfile;
        }

        public async Task<UserProfileDTO?> LoginUser(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.Password))
                return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash))
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDTO.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            // probably generate JWT token here
            // probably generate JWT token here
            await _context.SaveChangesAsync();
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Avatar = user.AvatarURL,
                JoinedOn = user.JoinedOn
            };
            return userProfile;
        }

        public async Task<UserProfileDTO?> UpdateUser(int Id, UpdateUserDTO updateUser, HttpRequest request)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user is null)
                return null;

            if (!string.IsNullOrWhiteSpace(updateUser.FullName))
                user.FullName = updateUser.FullName;

            if (updateUser.Avatar != null)
            {
            var uploaddir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploaddir))
                Directory.CreateDirectory(uploaddir);

            var filename = $"{Guid.NewGuid()}_{updateUser.Avatar.FileName}".Replace(" ","");
            var filepath = Path.Combine(uploaddir, filename);
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await updateUser.Avatar.CopyToAsync(stream);
            }
                // Generate the file URL
            user.AvatarURL = $"{request.Scheme}://{request.Host}/images/{filename}";
            }

            // probably generate JWT token here
            await _context.SaveChangesAsync();
            var userProfile = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Avatar = user.AvatarURL,
                JoinedOn = user.JoinedOn
            };
            return userProfile;
        }
    }
}