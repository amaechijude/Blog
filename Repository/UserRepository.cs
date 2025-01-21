using Blog.DataContext;
using Blog.DTOs;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly AppDbContext _context = context;

        public async Task<UserProfileDTO> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserProfileDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                   FullName = u.FullName,
                   JoinedOn = u.JoinedOn,
                   Posts = u.Posts.Select(p => new PostViewDTO
                   {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        ImageUrl = p.ImageUrl,
                        Likes = p.Likes,
                        CreatedAt = p.CreatedAt,
                        LastUpdatedAt = p.LastUpdatedAt,
                        UserId = p.UserId
                   }).ToList()
                })
                .FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("User not found or is deleted");
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string userEmail)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
            return user is null? null : user;
        }

    }
}