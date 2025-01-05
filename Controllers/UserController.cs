using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.DataContext;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user is null)
                return NotFound("User doest not exist or is deleted");

            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
    }
}