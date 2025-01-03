using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Model;
using Microsoft.AspNetCore.Identity;

namespace Blog.Repository
{
    public class UserRepository(AppDbContext context)
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly AppDbContext _context = context;

        public async Task<User?> GetUserById(int Id)
        {
            var user = await _context.Users.FindAsync(Id);
            if (user is null)
                return null;

            return user;
        }

        public async Task<User?> RegisterUser(UserDTO userDTO)
        {
            var user = new User
            {
                Email = userDTO.Email,
                JoinedOn = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}