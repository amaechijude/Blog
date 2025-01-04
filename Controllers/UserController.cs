using Blog.DTOs;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class UserController(UserRepository userRepository) : ControllerBase
    {
       private readonly UserRepository _userRepository = userRepository;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            var user = await _userRepository.RegisterUser(userDTO);
            if (user is null)
                return BadRequest("Invalid user data");
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(UserDTO userLogin)
        {
            var user = await _userRepository.LoginUser(userLogin);
            if (user is null)
                return BadRequest("Invalid user data");
            return Ok(user);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromForm] UpdateUserDTO updateUser)
        {
            var user = await _userRepository.UpdateUser(id, updateUser, Request);
            if (user is null)
                return BadRequest("User not found");
            return Ok(user);
        }
    }
}
