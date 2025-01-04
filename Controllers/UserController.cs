using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserRepository userRepository) : ControllerBase
    {
        private readonly UserRepository _userRepository = userRepository;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user is null)
                return NotFound("User doest not exist or is deleted");

            return Ok(user);
        }
    }
}