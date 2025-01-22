using Blog.DTOs;
using Blog.services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try {
                return Ok(await _userService.RegisterUser(registerUser));
                }
            
            catch (Exception ex)
            {
                return BadRequest(new {
                    status = "faliled",
                    message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                return Ok(await _userService.GetUserByIdAsync(id));
            }
            catch (KeyNotFoundException ex) 
            {
                 return BadRequest(new {
                    status = "faliled",
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserDTO loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState["errors"]);
            try
            {
                return Ok(await _userService.LoginUserAsync(loginUser));
            }
            catch (Exception ex)
            {
                 return BadRequest(new {
                    status = "faliled",
                    message = ex.Message
                });
            }
        }
    }
}