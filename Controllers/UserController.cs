using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
using Blog.DTOs;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserRepository userRepository, AppDbContext context) : ControllerBase
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly AppDbContext _context = context;

        [HttpPost("create")]
        public async  Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = await _userRepository.RegisterUser(registerUser);
            if (newUser is null)
                return BadRequest("User not registered");
            
            return Ok(newUser);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user is null)
                return NotFound("User doest not exist or is deleted");

            List<PostViewDTO> posts = await _context.Posts
                .Where(uid => uid.UserId == user.Id)
                .Select(p => new PostViewDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    Likes = p.Likes,
                    CreatedAt = p.CreatedAt,
                    LastUpdatedAt = p.LastUpdatedAt,
                    UserId = p.UserId

                })
                .ToListAsync();
            var userView = new UserProfileDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                AvatarURL = user.AvatarURL,
                JoinedOn = user.JoinedOn,
                Posts = posts
            };
            return Ok(userView);
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
    }
}